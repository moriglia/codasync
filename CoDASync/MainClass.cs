using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CoDASync
{
    class MainClass
    {
        public static void Main(string[] args){
            //CorvusManager.CorvusManagerTest();
            //DAQManager.TestSingleAcquisition(); // OK funziona

            //DAQManager DM = new DAQManager();
            //DM.TestContinuousAcquisition();

            //Application.EnableVisualStyles();
            //Application.DoEvents();
            //Application.Run(new MainForm());
			
			DAQManager.TestDAQManager();
        }
    }
}
