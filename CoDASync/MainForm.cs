using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Application.EnableVisualStyles();
            Application.DoEvents();
            Application.Run(new CoDASyncWindow());
        }
		
		public void ConnectPortButton_Click(object sender, EventArgs e){
			try {
				CorvusManager tempCM = new CorvusManager(PortTextBox.Text, Int32.Parse(BaudRateTextBox.Text));
				CM = tempCM;
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
			if (IsCMSet)
				CM.sendCommand(venusCommand);
        }

        private void SendCommandButton_Click(object sender, EventArgs e)
        {
            sendVenusCommand(VenusCommandBox.Text);
        }

		// relative move section ------------------------------------------------
		private void RelativeMove_Click(object sender, EventArgs ea)
		{
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
				
				// Move all button
				default:
					x = (float)this.RMoveX.Value;
					y = (float)this.RMoveY.Value;
					z = (float)this.RMoveZ.Value;
					break;
			}
			CM.rmove(x, y, z);
		}
		
		// ----------------------------------------------------------------------
        
		private void VenusCommandBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyCode == 13) // enter key pressed
            {
                sendVenusCommand(VenusCommandBox.Text);
            }
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {

        }

        private void KeyUpHandler(object sender, KeyEventArgs e)
        {

        }
    }
}
