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
		
		
		private delegate void SafeCallDelegate(ref string line);
		
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
				foreach(double d in sam)
				{
					sample += d.ToString() + " ";
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
				
				
				lock(__doneLocker) done = true;
				
				__bufferReadyCountdown.Reset();
			}
		}
		
		public void displayLineToLog(ref string line)
		{
			if (this.CorvusEventDisplay.InvokeRequired)
			{
				var d = new SafeCallDelegate(displayLineToLog);
				Invoke(d, new object [] { line });
			}
			else
			{
				this.CorvusEventDisplay.Text += line;
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
			if (IsCMSet)
				if (CM.IsOpen)
					this.CorvusConnectionLabel.Text = "Open";
				else
					this.CorvusConnectionLabel.Text = "Closed (configured)";
			else
				this.CorvusConnectionLabel.Text = "Not configured";
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
				for ( int i = 0; low <= high; ++low) 
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
			if (this.IsDMSet) this.DeviceConfigurationStatusLabel.Text = "Configured";
			else this.DeviceConfigurationStatusLabel.Text = "Configured";
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
        
		private void VenusCommandBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyCode == 13) // enter key pressed
            {
                sendVenusCommand(VenusCommandBox.Text);
            }
        }
		
		private void BrowseFileButton_Click(Object senderk, EventArgs ea)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Select an output file";
			ofd.Filter = "Text Files (*.txt)|All Files(*.*)";
			ofd.CheckFileExists = false;
			if (ofd.ShowDialog() == DialogResult.OK)
				OutputFileTextBox.Text = ofd.FileName;
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
				this.SamplingStatusLabel.Text = "Sampling ...";
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
				this.SamplingStatusLabel.Text = "Ready";
			}
		}
    }
}
