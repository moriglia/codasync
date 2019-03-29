using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using NationalInstruments.DAQmx;

namespace CoDASync
{
    class DAQManager
    {
        private Task acquisitionTask; //N. I. Task. It uses analogMultiChallenReader to get data from control board of the load cell.
        private AnalogMultiChannelReader analogMultiChannelReader;
		private double rate;
        private NationalInstruments.AnalogWaveform<double>[] data;
		private AsyncCallback onContinuousDataAcquiredCallback = null;
		
		
		// used to signal that acquisition is completed and data is available to consume
		private EventWaitHandle acquisitionReady;

		
		// constructor
		public DAQManager (String _device, int [] _channels, ref EventWaitHandle _acquisitionReady)
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
			
			// acquire reference to "condition variable"
			acquisitionReady = _acquisitionReady;
			
			// set callback for continuous acquisition handling
			onContinuousDataAcquiredCallback = new AsyncCallback(OnContinuousDataAcquired);
			analogMultiChannelReader.SynchronizeCallbacks = true;
		}
		
		// start single sample acquisition
		public void StartAcquisition(ref double[] _data)
		{	
			// read a sample
			_data = analogMultiChannelReader.ReadSingleSample();
			
			// signal that data is ready
			acquisitionReady.Set();
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
			// put acquired data in the buffer provided by the caller of StartAcquisition (function below)
			data = analogMultiChannelReader.EndReadWaveform(iar);
			
			// signal that data is ready
			acquisitionReady.Set();
		}
		
		// start continuous acquisition
		public void StartAcquisition(ref NationalInstruments.AnalogWaveform<double>[] _data, int sampleNumber)
		{
			data = _data;
			
			if (rate==0)
			{	// if no acquisition rate has been set, set it to default
				ConfigureContinuousAcquisitionClockRate(0);
			}
			
			analogMultiChannelReader.BeginReadWaveform(
				sampleNumber, // number of samples per channel that will be acquired
				onContinuousDataAcquiredCallback, // callback at the end of the acquisition
				acquisitionTask
			);
		}
		
		public static void TestDAQManager()
		{
			EventWaitHandle dataReady = new AutoResetEvent(false);
			int[] voltage_channels = { 0, 1, 2, 3, 4, 5 };
			DAQManager DM = new DAQManager(
				"Dev2",
				voltage_channels, // device channels
				ref dataReady // condition variable to use for synchronization with data acquirer
			);
			DM.ConfigureContinuousAcquisitionClockRate(1000);
			NationalInstruments.AnalogWaveform<double>[] test_data = new NationalInstruments.AnalogWaveform<double>[1];
			DM.StartAcquisition(ref test_data, 15);
			
			// wait for data acquisition completion
			dataReady.WaitOne();
			
			// print data
			int j = 0;
			foreach (NationalInstruments.AnalogWaveform<double> aw in test_data)
			{
				double [] channel_data = aw.GetRawData(0, aw.SampleCount);
				for(int i = 0; i < aw.SampleCount; ++i)
				{
					Console.WriteLine("Channel " + j.ToString() + "\tSample " + i.ToString() + "\tValue " + channel_data[i].ToString());
				}
				++j;
			}
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
