using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Timers;
using System.IO;
using System.Diagnostics;

namespace CoDASync
{
    public partial class CoDASyncWindow : Form
    {
        CorvusManager CM;
		protected bool IsCMSet;
		
		DAQManager DM;
		protected bool IsDMSet;
		
		// signaling and locking variables for acquisition completed and stored
		protected Object __doneLocker; 
		protected bool done;
		
		
		// LISTING EXECUTION -------------------------------------------
		// is a listing execution in progress ?
		protected bool execInProgress;
		private Thread executionThread;
		protected object __execThreadLocker; 
		// END of LISTING EXECUTION ------------------------------------
		
		
		
		// MEMBERS TO HANDLE TIMED ACQUISITION ------------------------------------------------
		
		// synchronization variable for Corvus serial acquisition
		protected AutoResetEvent __readSerialEvent;
		
		// synchronization variable for NIDAQmx acquisition
		protected AutoResetEvent __readDAQEvent;
		
		// synchronization countdown for storing the read buffers
		protected CountdownEvent __bufferReadyCountdown;
		
		// buffers for acquiring data
		protected String position;
		protected String sample;
		protected String [] sample_array;
		protected String time;
		
		private String output;
		
		// name of file where to output data.
		protected String outputFileName;
		protected StreamWriter outputStream;
		protected Object __outputFileStreamLocker;
		
		// timer for periodical acquisition
		protected Object __timerLocker;
		protected System.Timers.Timer samplingTimer;
		protected bool acquisitionInProgress;
		
		// acquisition tasks
		private Thread readSerialThread, readDAQThread, storeDataThread;
		
		// END OF MEMBERS TO HANDLE TIMED ACQUISITION ----------------------------------------- 
		
		
		private delegate void SafeDisplayLineToLogDelegate(ref string line);
		private delegate void SafeUpdateCorvusConnectionStatusDelegate();
		private delegate void SafeSetText(Control c, String text);
		
		// Constructor
        public CoDASyncWindow()
        {
            InitializeComponent();
			
            CM = null; //new CorvusManager("COM6", 57600);
			IsCMSet = false;
			
			DM = null;
			IsDMSet = false;
			
			// initialize locker for signaling condition that computation is done
			done = true;
			__doneLocker = new Object();
			
			
			__readSerialEvent = new AutoResetEvent(false);
			__readDAQEvent  = new AutoResetEvent(false);
			__bufferReadyCountdown = new CountdownEvent(3);
			
			
			position = null;
			sample = null;
			sample_array = null;
			time = null;
			output = "";
			
			// output file name
			outputFileName = "";
			outputStream = null;
			__outputFileStreamLocker = new Object();
			
			// configure timer general parameters 
			samplingTimer = new System.Timers.Timer();
			__timerLocker = new Object();
			samplingTimer.Elapsed += TimerHandler;
			samplingTimer.AutoReset = true;
			acquisitionInProgress = false;
			
			// create threads
			readSerialThread = null;
			readDAQThread = null;
			storeDataThread = null;
			
			// listing variables
			execInProgress = false;
			executionThread = null;
			__execThreadLocker = new Object();
        }
		
		
		
		
		// PERIODICAL ACQUISITION FUNCTIONS ------------------------------------------------------
		
		// called in response to timer event
		protected void TimerHandler(Object sender, ElapsedEventArgs eea)
		{
			// gain lock to check whether the previous data has been saved
			lock(__doneLocker)
			{
				// check whether data has been saved
				while(!done)
					// temporarly release lock in order to allow done to be changed by Store()
					Monitor.Wait(__doneLocker);
					// when monitor.wait exits the lock is acquired back
					
				// we can set done to false and proceed
				done = false;
			}
			
			// tell the acquisition threads that they can proceed
			__readSerialEvent.Set();
			__readDAQEvent.Set();
			
			time = (DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss.fff");
			__bufferReadyCountdown.Signal();
		}
		
		// must be run as thread
		protected void ReadSerial()
		{
			while (true)
			{				
				__readSerialEvent.WaitOne();
				
				// assume that CM has been correctly initialized
				if (!CM.pos(ref position))
				{
					// handle error
				}
				
				__bufferReadyCountdown.Signal();
			}
		}
		
		// must be run as thread
		protected void ReadDAQmx()
		{
			double[] sam;
			while(true)
			{
				__readDAQEvent.WaitOne();
				
				// assume that DM has been correctly initialized
				sam = DM.Acquire();
				
				sample = "";
				sample_array = new String[sam.Length];
				for(int i = 0; i<sam.Length; ++i)
				{
					sample += sam[i].ToString() + " ";
					sample_array[i] = sam[i].ToString();
				}
				
				__bufferReadyCountdown.Signal();
			}
		}
		
		
		// must be run as thread
		protected void Store()
		{
			while(true)
			{
				__bufferReadyCountdown.Wait();
				
				try
				{
					outputStream.WriteLine(
						time + 
						Regex.Replace(
							Regex.Replace(
								position,
								@"[\n\r]*",
								""
							)
							+ " " + sample, 
							@"\s+(?!$)", 
							","
						)
					);					
				} catch (Exception e) {
					// stop all threads
					Console.WriteLine("Stopping all threads...");
				}
				
				displayForce();
				
				__bufferReadyCountdown.Reset();
				
				lock(__doneLocker) done = true;
			}
		}
		
		public void displayLineToLog(ref string line)
		{
			if (this.CorvusEventDisplay.InvokeRequired)
			{
				var d = new SafeDisplayLineToLogDelegate(displayLineToLog);
				Invoke(d, new object [] { line });
			}
			else
			{
				this.CorvusEventDisplay.Text += line + "\r\n";
			}
		}
		
		protected void displayForce()
		{
			/*	this function is unsafe, must be called in a safe context:
				since it reads the variable @sample_array, which is shared between multiple threads
				the caller must call displayForce() in mutual exclusion with the threads that 
				use @sample_array
			*/

			switch(sample_array.Length)
			{
				/*
					Why all those "goto case <next>"? Blame C# creators...
					https://stackoverflow.com/questions/6696692/control-cannot-fall-through-from-one-case-label
				*/
				default:
					goto case 6;
				case 6:
					SetText(this.TzTextBox, sample_array[5]);
					goto case 5;
				case 5:
					SetText(this.TyTextBox, sample_array[4]);
					goto case 4;
				case 4:
					SetText(this.TxTextBox, sample_array[3]);
					goto case 3;
				case 3:
					SetText(this.FzTextBox, sample_array[2]);
					goto case 2;
				case 2:
					SetText(this.FyTextBox, sample_array[1]);
					goto case 1;
				case 1:
					SetText(this.FxTextBox, sample_array[0]);
					break;
				case 0:
					break;
			}
		}

		
		public void TestPeriodicalAcquisition()
		{
			
			CM = new CorvusManager("COM6", 57600);
			IsCMSet = true;
			
			int[] channels = {0,1,2,3,4,5};
			DM = new DAQManager("Dev2", channels);
			IsDMSet = true;
			
			
			System.Timers.Timer t = new System.Timers.Timer(1000);
			t.Elapsed += TimerHandler;
			t.AutoReset = true;
			
			
			Thread thread_store = new Thread(Store);
			thread_store.Start();
			
			Thread thread_serial = new Thread(ReadSerial);
			thread_serial.Start();
			
			Thread thread_daq  = new Thread(ReadDAQmx);
			thread_daq.Start();
			
			t.Enabled = true;
			
			Thread.Sleep(10000);
			
			thread_store.Abort();
			thread_daq.Abort();
			thread_serial.Abort();
			
			Console.WriteLine("Test ended, press enter to exit.");
			Console.Read();
			
			File.WriteAllText("D:\\OrigliaMarco\\CoDASync\\Acquisition.txt", output);
		}
		
		
		// END OF PERIODICAL ACQIOSITION FUNCTIONS -----------------------------------------------------------
		
		
		[STAThread]
        public static void Main()
        {
			//CorvusManager.CorvusManagerTest();
			//CorvusManager.CorvusManagerTestPos();
			
            Application.EnableVisualStyles();
            Application.DoEvents();
            Application.Run(new CoDASyncWindow());
			
			//CoDASyncWindow csw = new CoDASyncWindow();
			//csw.TestPeriodicalAcquisition();
        }
		
		public void ConnectPortButton_Click(object sender, EventArgs e){
			try {
				if (IsCMSet && CM.IsOpen) CM.Close();
				CM = new CorvusManager(PortTextBox.Text, Int32.Parse(BaudRateTextBox.Text));
				IsCMSet = true;
			} catch (Exception ex){
				MessageBox.Show(
					"Invalid port or baud rate configuration", 
					"Corvus Configuration Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
			this.updateCorvusConnectionStatus();
			
			CM.sc = displayLineToLog;
		}
		
		protected void updateCorvusConnectionStatus()
		{
			/*if (this.CorvusConnectionLabel.InvokeRequired)
			{
				return;
				var d = new SafeUpdateCorvusConnectionStatusDelegate(updateCorvusConnectionStatus);
				Invoke(d, null);
			} else
				if (IsCMSet)
					if (CM.IsOpen)
						this.CorvusConnectionLabel.Text = "Open";
					else
						this.CorvusConnectionLabel.Text = "Closed (configured)";
				else
					this.CorvusConnectionLabel.Text = "Not configured";
			*/
			if (IsCMSet)
				if (CM.IsOpen)
					SetText(this.CorvusConnectionLabel,"Open");
				else
					SetText(this.CorvusConnectionLabel,"Closed (conf'd)");
			else
				SetText(this.CorvusConnectionLabel, "Not configured");
			
		}

        public void sendVenusCommand(String venusCommand)
        {
			// check whether port is open
			if (!IsCMSet)
			{
				MessageBox.Show(
					"Not Connected to Corvus Platform through serial port", 
					"No port connection error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				return ;
			} 
			
			CM.sendCommand(venusCommand);
			
			this.updateCorvusConnectionStatus();
        }

        private void SendCommandButton_Click(object sender, EventArgs e)
        {
            sendVenusCommand(VenusCommandBox.Text);
			this.updateCorvusConnectionStatus();
        }

		// relative move section ------------------------------------------------
		private void RelativeMove_Click(object sender, EventArgs ea)
		{
			// check whether port is open
			if (!IsCMSet)
			{
				MessageBox.Show(
					"Not Connected to Corvus Platform through serial port", 
					"No port connection error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				return ;
			}
			
			// get differential coordinates from user input
			float x = 0, y = 0, z = 0;
			String txt = ((System.Windows.Forms.Button)sender).Text;
			switch(txt)
			{
				// x+ and x- buttons
				case "x+":
					x =  (float)this.RMoveX.Value;
					break;
				case "x-":
					x = -(float)this.RMoveX.Value;
					break;
				
				// y+ and y- buttons
				case "y+":
					y =  (float)this.RMoveY.Value;
					break;
				case "y-":
					y = -(float)this.RMoveY.Value;
					break;
					
				// z+ and z- buttons
				case "z+":
					z =  (float)this.RMoveZ.Value;
					break;
				case "z-":
					z = -(float)this.RMoveZ.Value;
					break;
				
				// Move all axes button
				default:
					x = (float)this.RMoveX.Value;
					y = (float)this.RMoveY.Value;
					z = (float)this.RMoveZ.Value;
					break;
			}
			
			// send command to platform
			CM.rmove(x, y, z);
			this.updateCorvusConnectionStatus();
		}
		
		// ----------------------------------------------------------------------
		
		
		// Set origin -----------------------------------------------------------
		public void SetOriginButton_Click(object sender, EventArgs ea)
		{
			// check whether port is open
			if (!IsCMSet)
			{
				MessageBox.Show(
					"Not Connected to Corvus Platform through serial port", 
					"No port connection error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				return ;
			}
			
			// get origin coordinates from user input
			float x = (float)this.OriginX.Value;
			float y = (float)this.OriginY.Value;
			float z = (float)this.OriginZ.Value;
			
			// send position to Corvus platform
			CM.setpos(x, y, z);
			this.updateCorvusConnectionStatus();
		}
		// ----------------------------------------------------------------------
		
		// Move to origin -------------------------------------------------------
		public void MoveToOriginButton_Click(object sender, EventArgs ea)
		{
			// check whether port is open
			if (!IsCMSet)
			{
				MessageBox.Show(
					"Not Connected to Corvus Platform through serial port", 
					"No port connection error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				return ;
			}
			
			CM.move(0,0,0);
			this.updateCorvusConnectionStatus();
		}
		
		// Move to position ------------------------------------------------------
		public void MoveToPositionButton_Click(object sender, EventArgs ea)
		{
			// check whether port is open
			if (!IsCMSet)
			{
				MessageBox.Show(
					"Not Connected to Corvus Platform through serial port", 
					"No port connection error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				return ;
			}
			
			// get origin coordinates from user input
			float x = (float)this.OriginX.Value;
			float y = (float)this.OriginY.Value;
			float z = (float)this.OriginZ.Value;
			
			CM.move(x,y,z);
			this.updateCorvusConnectionStatus();
		}
		
		
		//---------------------------------------------------------------------------+
		// NI DAQ mx section                                                         |
		//---------------------------------------------------------------------------+
		
		
		// string parser for getting channels to use for DAQmx data acquisition
		protected int [] parseIntRange(String s)
		{
			// regex for ranges e.g: "13 - 20", "13- 20", "13   -20", "13-20"
			String pattern_range = @"(\d+)\s*-\s*(\d+)";
			
			// regex for single natural numbers,
			// they may be preceded or followed by anything but a "-" (which will match ranges)
			String pattern_single = @"(?<!-\s*)(\d+)(?!\s*-)";
			
			List<int> tmp = new List<int>();
			int length = 0;
			
			// get all matches for ranges like "5 - 12"
			foreach(Match m in Regex.Matches(s, pattern_range))
			{
				// get range boundaries
				int low = int.Parse(m.Groups[1].Value);
				int high = int.Parse(m.Groups[2].Value);
				
				// reorder boundaries
				if (low > high) 
				{
					int temp = low;
					low = high;
					high = temp;
				}
				
				// insert data into array
				for ( /*int i = 0*/; low <= high; ++low) 
				{
					tmp.Add(low);
					++length;
				}
			}
			
			// get all matches for single values like "... , 3, 5 12, 9 / 44, ..."
			// the example below will get : 3, 5, 12, 9, 44
			foreach (Match m in Regex.Matches(s, pattern_single))
			{
				int v = int.Parse(m.Groups[0].Value);
				
				tmp.Add(v);
				++length;
			}
			
			// reorder array 
			tmp.Sort();
			
			// remove duplicates;
			for (int i = 1; i<length; )
			{
				if (tmp[i] == tmp[i-1])
				{
					tmp.RemoveAt(i);
					--length;
					continue;
				}
				++i;
			}
			
			return tmp.ToArray();
		}
		
		
		protected void updateDeviceStatusLabel()
		{
			if (this.IsDMSet) SetText(this.DeviceConfigurationStatusLabel, "Configured");
			else SetText(this.DeviceConfigurationStatusLabel, "Configured");
		}
		
		// get necessary parameters and set the NIDAQmx manager
		public void ConfigureNIDAQmxButton_Click(object sender, EventArgs ea)
		{
			int[] channels = parseIntRange(this.ChannelTextBox.Text);
			String device = this.DeviceNameTextBox.Text;
			
			DM = new DAQManager(device, channels);
			
			IsDMSet = true;
			this.updateDeviceStatusLabel();
		}
		
		public void CalibrateUsingMatrixFileButton_Click(Object sender, EventArgs ea)
		{
			// check for configuration
			if (!IsDMSet)
			{
				MessageBox.Show(
					"Configure device first",
					"Missing configuration",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				return;
			}
			
			// select file that contains calibration matrix
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Select a calibration file";
			ofd.CheckFileExists = true;
			if (ofd.ShowDialog() != DialogResult.OK)
				return;
			
			
			// read the file
			string fileContent = null;
			try
			{
				fileContent = File.ReadAllText(ofd.FileName);
			} catch (Exception e)
			{
				MessageBox.Show("Something went wrong opening the specified file: " + ofd.FileName, "Wrong path", 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			
			// parse the file
			Match m = Regex.Match(fileContent, @"calibration\s*:\s*(?:(?:([+-]?\d+(?:\.\d+)?)\s*,?\s*){6}\s*;?\s*){6}");
			if (!m.Success)
			{
				MessageBox.Show(
					"Unrecognized matrix calibration file format, the calibration file must match this RegEx: " +@"calibration\s*:\s*(?:(?:([+-]?\d+(?:\.\d+)?)\s*,?\s*){6}\s*;?\s*){6}",
					"Unrecognized format",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				return ;
			}
			
			//CM.InitializeMatrixSize(6);
			
			
			for (int i = 0; i < 6; ++i)
				for (int j = 0; j<6; ++j )
					DM.calibrationMatrix[i,j] = double.Parse(m.Groups[1].Captures[6*i + j].Value);
		}
		
		public void EvaluateBiasButton_Click(Object sender, EventArgs ea)
		{
			if(!IsDMSet)
			{
				MessageBox.Show("No device configured", "Device configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			
			DM.SetBias();
		}
        
		private void VenusCommandBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.C)
			{
				sendVenusCommand(((char)4).ToString());
				return ;
			}
			
			if((int)e.KeyCode == 13)
			{
				sendVenusCommand(VenusCommandBox.Text);
				return ;
			}
        }
		
		private void CorvusEventDisplay_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.C)
			{
				sendVenusCommand(((char)4).ToString());
				return ;
			}
			
			return;
        }
		
		private void BrowseListingButton_Click(Object sender, EventArgs ea)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Select a listing file";
			//ofd.Filter = "Text Files (*.txt)|All Files(*.*)";
			ofd.CheckFileExists = true;
			if (ofd.ShowDialog() == DialogResult.OK)
				SetText(ListingFilename, ofd.FileName);
		}
		
		private void ExecListingButtonWorker()
		{
			lock (__execThreadLocker) 
			{
				if (execInProgress)
				{	// abort execution
					if (executionThread.IsAlive)
						executionThread.Abort();
					executionThread = null;
					SetText(ExecListingButton, "Exec Listing");
					execInProgress = false;
					return ;
				}

				// start execution
				executionThread = new Thread(parseListing);
				executionThread.Start();
				SetText(ExecListingButton,"Abort Listing");
				execInProgress = true;
			}
					
			try
			{
				executionThread.Join();
			} catch (Exception e ) {
				// nothing to do
				// just to catch ThreadInterruptedException
			}

			lock (__execThreadLocker) {
				SetText(ExecListingButton, "Exec Listing");
				execInProgress = false;
				executionThread = null;
			}
		}
		
		private void ExecListingButton_Click(Object sender, EventArgs ea)
		{
			(new Thread(ExecListingButtonWorker)).Start();
			
			return;
		}
		
		private void parseListing()
		{
			/*StreamReader listingFile;
			try
			{	// open file if it exists
				listingFile = new StreamReader(ListingFilename.Text);
			} catch (Exception e)
			{	// problems in opening the file
				MessageBox.Show(
					"The specified filename is not valid: " + ListingFilename.Text,
					"File not found",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				
				// our job is completed
				return;
			}*/
			
			// read line by line
			string[] lines =  null;
			try
			{
				lines = File.ReadAllLines(ListingFilename.Text);
			}catch(Exception e)
			{
				MessageBox.Show(
					"The specified filename is not valid: " + ListingFilename.Text,
					"File not found",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				return ;
			}
			
			foreach(string line in lines)
			{
				parseListingLine(line);
			}
			
			// clean up stream
			//listingFile.Close();
			
			return;
		}
		
		protected void parseListingLine(String line)
		{
			/* 	look for sleep directives:
				Matches this format: 
					"<compulsory spacing>.sleep<mandatory space + more compulsory spacing><one non-zero digit><more compulsory digits><compulsory spacing>"
				The following will match
					".sl 500 "
					".sleep     500  "
					"    .sl   500"
				The following will not match:
					".sle 500"
					".sleep 500 other stuff"
					" nasty stuff .sl 500"
			*/
			Match m = Regex.Match(line, @"^\s*\.sl(?:eep)?\s+([1-9]\d*)\s*$");
			if (m.Success)
			{	// a match is found
				Thread.Sleep(int.Parse(m.Groups[1].Value));
				return;
			}
			
			// if the line is not a sleep directive, it may be a venus command
			sendVenusCommand(line);
			
			return;
		}
		
		private void BrowseFileButton_Click(Object sender, EventArgs ea)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Select an output file";
			ofd.Filter = "Text Files (*.txt)|All Files(*.*)";
			ofd.CheckFileExists = false;
			if (ofd.ShowDialog() == DialogResult.OK)
				SetText(OutputFileTextBox, ofd.FileName);
		}
		
		
		
		
		protected void SetText(Control c, String text)
		{
			if (c.InvokeRequired)
			{
				var d = new SafeSetText(SetText);
				Invoke(d, new object[] {c, text});
			} else 
				c.Text = text;
		}
		
		
		
		private void StartAcquisitionButton_Click(Object sender, EventArgs ea)
		{
			if (!IsCMSet || !IsDMSet)
			{
				MessageBox.Show(
					"Devices not configured yet"
				);
				return ;
			}
		
			lock(__timerLocker)
			{	
				if (acquisitionInProgress)
				{
					// handle the fact that an acquisition is aready running
					MessageBox.Show(
						"Cannot start a new acquisition. An acquisition is already running. Stop it before you proceed!", 
						"Acquisition in progress",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					return ;
				}
				
				if (SamplingPeriodBox.Value == 0)
				{
					MessageBox.Show(
						"A period of 0 would seem a bit as a continuous sampling, which I'm not going to do :(", 
						"Invalid period",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					return ;
				}
				
				// configure output stream
				try{
					outputStream = new StreamWriter(OutputFileTextBox.Text);
				}catch (Exception e){
					// handle me
				}
				
				// reset countdown
				__bufferReadyCountdown.Reset();
				
				// start threads that cooperate in the acquisition tasks
				Debug.Assert(readSerialThread==null && readDAQThread==null && storeDataThread==null);
				
				readSerialThread = new Thread(ReadSerial);
				readDAQThread = new Thread(ReadDAQmx);
				storeDataThread = new Thread(Store);

				readSerialThread.Start();
				readDAQThread.Start();
				storeDataThread.Start();
				
				// wait a bit to make sure the threads are ready to receive signals
				Thread.Sleep(500);
				
				//Configure timer period and start acquisition
				samplingTimer.Interval = (double)SamplingPeriodBox.Value;
				samplingTimer.Enabled = true;
				
				// set mark acquisition as in progress
				acquisitionInProgress = true;
				SetText(this.SamplingStatusLabel, "Sampling ...");
			}
		}
		
		private void StopAcquisitionButton_Click(Object sender, EventArgs ea)
		{
			lock(__timerLocker)
			{
				if (!acquisitionInProgress)
				{
					MessageBox.Show(
						"No acquisition in progress. Start a new one pressing button \'Start\'",
						"No acquisition in progress",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Warning
					);
					return ;
				}
				
				samplingTimer.Enabled = false;
				
				// stop threads that are waiting for synchronization
				readSerialThread.Abort();
				readDAQThread.Abort();
				storeDataThread.Abort();
				
				readSerialThread = null;
				readDAQThread = null;
				storeDataThread = null;
				
				// close output stream
				try {
					outputStream.Close();
					outputStream = null;
				}catch (Exception e) {
					// handle me
				}
				
				acquisitionInProgress = false;
				SetText(this.SamplingStatusLabel,"Ready");
			}
		}
    }
}
