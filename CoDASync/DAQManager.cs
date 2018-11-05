using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using NationalInstruments.DAQmx;

namespace CoDASync
{
    class DAQManager
    {
        private Task acquisitionTask;
        private AnalogMultiChannelReader analogMultiChannelReader;
        private NationalInstruments.AnalogWaveform<double>[] data;


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

                acquisitionTask.Timing.ConfigureSampleClock(
                    "", //internal clock
                    1000,
                    SampleClockActiveEdge.Rising, 
                    SampleQuantityMode.ContinuousSamples,
                    1000
                );

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
