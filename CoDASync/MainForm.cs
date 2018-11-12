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
    public partial class MainForm : Form
    {
        CorvusManager CM;
        public MainForm()
        {
            InitializeComponent();
            CM = new CorvusManager("COM6", 57600);
        }

        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.DoEvents();
            Application.Run(new MainForm());
        }

        public void sendVenusCommand(String venusCommand)
        {
            CM.sendCommand(venusCommand);
        }

        private void VenusCommandBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void SendCommandButton_Click(object sender, EventArgs e)
        {
            CM.sendCommand(VenusCommandBox.Text);
        }

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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
