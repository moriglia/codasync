using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using NationalInstruments.DAQmx;

using System.IO;

namespace CoDASync
{
	// marked as unsafe to use pointers
    class DAQManager
    {
        private Task acquisitionTask; //N. I. Task. It uses analogMultiChannelReader to get data from control board of the load cell.
        private AnalogMultiChannelReader analogMultiChannelReader;
		private double rate;
        public NationalInstruments.AnalogWaveform<double>[] data
		{
			get ;
			private set ;
		}
		private int sampleNumber;
		private AsyncCallback onContinuousDataAcquiredCallback = null;
		
		
		// used to signal that acquisition is completed and data is available to consume
		private EventWaitHandle dataAvailable;
		private EventWaitHandle dataConsumed;
		
		// acquisition lock: this is used to guarantee that every acquisition is started in mutual exclusion with other acquisitions.
		private object acquisitionLock;

		
		// constructor
		public DAQManager (String _device, int [] _channels)
		{
			acquisitionTask = new Task();
			
			// initialize channels
			for (int i = 0; i < _channels.Length; ++i)
			{
				try
				{
					acquisitionTask.AIChannels.CreateVoltageChannel(
                        _device + "/ai" + _channels[i].ToString(),
                        "Channel_" + _channels[i].ToString(),
                        (AITerminalConfiguration)(-1),
                        -10,
                        10,
                        AIVoltageUnits.Volts
                    );
				} catch(DaqException de)
				{
					throw de;
				}
			}
			
			// initialize Channel Reader
			analogMultiChannelReader = new AnalogMultiChannelReader(acquisitionTask.Stream);
			
			// set rate to conventional value 0; If not set later (ConfigureContinuousAcquisitionClockRate) 
			// a default value will be used for continuous acquisition
			rate = 0;
			
			// copied from examples
			acquisitionTask.Control(TaskAction.Verify);
			
			// create condition variable for data available
			dataAvailable = new AutoResetEvent(false);
			// create condition variable for data consumed
			dataConsumed = new AutoResetEvent(true);
			
			// set callback for continuous acquisition handling
			onContinuousDataAcquiredCallback = new AsyncCallback(OnContinuousDataAcquired);
			analogMultiChannelReader.SynchronizeCallbacks = true;
			
			// create acquisition lock object
			acquisitionLock = new object();
			
			// no data at start
			data = null;
		}
		
		// acquire one sample
		public double[] Acquire()
		{	
			/*
				return value: array with one value per channel
			*/
			// read a sample
			lock(acquisitionLock) return analogMultiChannelReader.ReadSingleSample();
			
		}
		
		
		public double[,] Acquire(int samplesPerChannel)
		{
			/*
				samplesPerChannel: how many samples to get from the buffer.
					If set to -1, all available values of the buffer will be read
				return value: a 2D array. The first dimension represents the channel, 
					the second represents the <samplesPerChannel> values of the selected channel
			*/
			lock(acquisitionLock) return analogMultiChannelReader.ReadMultiSample(samplesPerChannel);
		}
		
		public void ConfigureContinuousAcquisitionClockRate(double _rate)
		{
			if (_rate!=0)
			{
				rate = _rate;
			} else {
				rate = 1000; //set to default
			}
			
			acquisitionTask.Timing.ConfigureSampleClock(
				"", // internal timer
				rate, // set rate or default,
				SampleClockActiveEdge.Rising, 
                SampleQuantityMode.ContinuousSamples
			);
		}
		
		// handler for end of acquisition
		protected void OnContinuousDataAcquired(IAsyncResult iar)
		{
			
			// create new AnalogWaveform object where to store the read data
			lock(acquisitionLock) data = analogMultiChannelReader.EndReadWaveform(iar);
			
			// signal that data is ready and ConsumeData can be called
			dataAvailable.Set();
		}
		
		// start continuous acquisition
		public void StartAcquisition(int _sampleNumber)
		{
			// data pointer may be necessary yet
			dataConsumed.WaitOne();
			
			sampleNumber = _sampleNumber;
			
			if (rate==0)
			{	// if no acquisition rate has been set, set it to default
				ConfigureContinuousAcquisitionClockRate(0);
			}
			
			lock(acquisitionLock) analogMultiChannelReader.BeginReadWaveform(
				sampleNumber, // number of samples per channel that will be acquired
				onContinuousDataAcquiredCallback, // callback at the end of the acquisition
				acquisitionTask
			);
		}
		
		// call this function after calling Start acquisition
		// and before usign Data
		public void WaitForData()
		{
			dataAvailable.WaitOne();
		}
		
		// call this function after you used Data
		public void SignalDataConsumed()
		{
			dataConsumed.Set();
		}
		
		public static void TestDAQManager()
		{
			int[] voltage_channels = { 0, 1, 2, 3, 4, 5 };
			DAQManager DM = new DAQManager(
				"Dev2",
				voltage_channels // device channels
			);
			DM.ConfigureContinuousAcquisitionClockRate(1000);
			DM.StartAcquisition(15);
			DM.WaitForData();
			NationalInstruments.AnalogWaveform<double>[] test_data = DM.data;
			
			// print data
			String outs = "";
			int j = 0;
			foreach (NationalInstruments.AnalogWaveform<double> aw in test_data)
			{
				double [] channel_data = aw.GetRawData(0, aw.SampleCount);
				for(int i = 0; i < aw.SampleCount; ++i)
				{
					outs += "Channel " + j.ToString() + "\tSample " + i.ToString() + "\tValue " + channel_data[i].ToString() + Environment.NewLine;
				}
				++j;
			}
			
			DM.SignalDataConsumed();
			
			File.WriteAllText(".\\Hello.txt", outs);
			
			Console.ReadLine();
			Thread.Sleep(20000);
		}

        public static void TestSingleAcquisition()
        {
			Task acquisitionTask;
			
			acquisitionTask = new Task();
			try{
                for (int i = 0; i < 6; ++i)
                {
                    acquisitionTask.AIChannels.CreateVoltageChannel(
                        "Dev2/ai" + i.ToString(),
                        "Acq" + i.ToString(),
                        (AITerminalConfiguration)(-1),
                        -10,
                        10,
                        AIVoltageUnits.Volts
                    );
                }

                
                /*String[] channelNames = new String[acquisitionTask.AIChannels.Count];
                int i = 0;
                foreach (AIChannel a in acquisitionTask.AIChannels)
                {
                    channelNames[i++] = a.PhysicalName;
                }*/

                AnalogMultiChannelReader analogMultiChannelReader = 
                    new AnalogMultiChannelReader(acquisitionTask.Stream);

                acquisitionTask.Control(TaskAction.Verify);

                double[] data =
                    analogMultiChannelReader.ReadSingleSample();

                foreach (double d in data)
                {
                    Console.WriteLine(d);
                }

			} catch(DaqException de) {
				Console.WriteLine(de);
				System.Threading.Thread.Sleep(1000);
			}

            
			//Console.WriteLine("Trovato Dispositivo: Dev2/ai0");
			System.Threading.Thread.Sleep(10000);
            Console.ReadLine();
		}

        public void TestContinuousAcquisition()
        {
			
			acquisitionTask = new Task();
			try{
                for (int i = 0; i < 6; ++i)
                {
                    acquisitionTask.AIChannels.CreateVoltageChannel(
                        "Dev2/ai" + i.ToString(),
                        "Acq" + i.ToString(),
                        (AITerminalConfiguration)(-1),
                        -10,
                        10,
                        AIVoltageUnits.Volts
                    );
                }

                acquisitionTask.Timing.ConfigureSampleClock(
                    "", //internal clock
                    1000,
                    SampleClockActiveEdge.Rising, 
                    SampleQuantityMode.ContinuousSamples
                );

                /*acquisitionTask.Timing.ConfigureSampleClock(
                    "", //internal clock
                    1000,
                    SampleClockActiveEdge.Rising, 
                    SampleQuantityMode.ContinuousSamples,
                    1000
                );*/

                acquisitionTask.Control(TaskAction.Verify);

                analogMultiChannelReader = 
                    new AnalogMultiChannelReader(acquisitionTask.Stream);

                AsyncCallback analogCallback =
                    new AsyncCallback(PrintData);

                analogMultiChannelReader.SynchronizeCallbacks = true;

                analogMultiChannelReader.BeginReadWaveform(
                    3, 
                    analogCallback,
                    acquisitionTask
                );



                /*double[] data =
                    analogMultiChannelReader.ReadSingleSample();

                foreach (double d in data)
                {
                    Console.WriteLine(d);
                }*/

			} catch(DaqException de) {
				Console.WriteLine(de);
				System.Threading.Thread.Sleep(1000);
			}

            
			//Console.WriteLine("Trovato Dispositivo: Dev2/ai0");
			System.Threading.Thread.Sleep(30000);
            Console.ReadLine();
		}

        private void PrintData(IAsyncResult iar)
        {
            data = analogMultiChannelReader.EndReadWaveform(iar);
            int j = 0;
            foreach (NationalInstruments.AnalogWaveform<double> AW in data)
            {
                Console.WriteLine("Channel " + (++j).ToString());
                for (int i = 0; i < AW.Samples.Count; ++i)
                {
                    Console.WriteLine(
                        j.ToString() + "\t"
                        + i .ToString() + "\t" 
                        + AW.Samples[i].ToString()
                    );
                    j++;
                }
            }
        }
    }
}
