﻿using System;
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
            //CorvusManagerTest();
            //DAQManager.TestSingleAcquisition(); // OK funziona

            DAQManager DM = new DAQManager();
            DM.TestContinuousAcquisition();
        }

        static void CorvusManagerTest()
        {
            CorvusManager cm = new CorvusManager("COM6", 57600);
            cm.rmove(-6, -5, -2);
            Thread.Sleep(5000);
            cm.rmove(6, 5, 2);
        }
    }
}
