using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace CoDASync
{
    public partial class CoDASyncWindow : Form
    {
        CorvusManager CM;
		protected bool IsCMSet;
		
		DAQManager DM;
		protected bool IsDMSet;
		
        public CoDASyncWindow()
        {
            InitializeComponent();
			
			//
            CM = null; //new CorvusManager("COM6", 57600);
			IsCMSet = false;
			
			DM = null;
			IsDMSet = false;
			
        }

        public static void Main()
        {
			//CorvusManager.CorvusManagerTest();
			CorvusManager.CorvusManagerTestPos();
            Application.EnableVisualStyles();
            Application.DoEvents();
            Application.Run(new CoDASyncWindow());
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
        }

        private void SendCommandButton_Click(object sender, EventArgs e)
        {
            sendVenusCommand(VenusCommandBox.Text);
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
		}
		
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
		
		// get necessary parameters and set the NIDAQmx manager
		public void ConfigureNIDAQmxButton_Click(object sender, EventArgs ea)
		{
			int[] channels = parseIntRange(this.ChannelTextBox.Text);
			String device = this.DeviceNameTextBox.Text;
			
			DM = new DAQManager(device, channels);
			
			IsDMSet = true;
		}
        
		private void VenusCommandBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyCode == 13) // enter key pressed
            {
                sendVenusCommand(VenusCommandBox.Text);
            }
        }
    }
}
