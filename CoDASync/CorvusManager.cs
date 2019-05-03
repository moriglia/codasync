using System;
using System.IO.Ports;
using System.Threading;
/*using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;*/

namespace CoDASync
{
    class CorvusManager : SerialPort
    {
		private Object __portWriteLock; // lock variable for writing on the port
		private Object __portReadLock;  // lock variable for reading from the port
		
		public delegate void stringConsumer(ref String str);
		
		public stringConsumer sc
		{
			set;
			protected get;
		}
		
        public CorvusManager(string portName, int baudRate) : base(portName, baudRate)
        {
			__portWriteLock = new Object();
			__portReadLock = new Object();
			
			sc = null;
			
			this.DataReceived += new SerialDataReceivedEventHandler(this.dataReceivedHandler);
        }
		
		protected void dataReceivedHandler(object sender, SerialDataReceivedEventArgs sdrea)
		{
			// if no consumer is set skip handling
			if (sc == null)
				return;
			
			String s;
			lock(__portReadLock)
			{
				/* we get in here only in the following cases:
				
				1 - No pos() has been called 
					==> we should consume the data
				
				2 - A pos() has released the lock after consuming a line
					==> we should check whether more data has to be consumed
					
				In no case we are receiving the data which were to be delivered
				to the caller of pos()
					
				Let's check whether data is available, we don't have to know 
				whether we were in case 1 or 2
				*/
				
				// set ReadLine nonblocking
				this.ReadTimeout = 0;
				
				try
				{
					// try reading something from port
					s = this.ReadLine();
				}
				catch(TimeoutException te)
				{
					// no data to read
					
					// reset ReadLine to blocking state
					this.ReadTimeout = -1;
					return;
				} 
				finally 
				{					
					this.ReadTimeout = -1;
				}
				
				// call string consumer that was externally defined
				sc(ref s);
			}
			
			return ;
		}

        protected void setOpen(){
           if(!IsOpen){
               Open();
           }
        }
		
		public bool sendCommand(String venusCommand){
			bool retval = true;
			// prevents multiple thread to write out of the port
			lock(__portWriteLock){
				try
				{
					setOpen();
					WriteLine(venusCommand);
				} catch (Exception ioe)
				{
					retval = false;
				}
			}
			
			return retval;
		}
		
		public static string packCommand(String command, params float[] parameters){
			String packedString = "";
			
			for(int i = 0; i < parameters.Length ; ++i){
				packedString += Convert.ToString(parameters[i]) + " ";
			}
			
			return packedString + command ;
		}
		
		
		// not public because it must be called in a critical section
		// where the __portReadLock has already been acquired
		protected bool readResponse(ref String s){
			bool retval = true;
			
			// read a line from the port			
			try
			{
				setOpen();
				s = ReadLine();
			} catch (Exception e){
				retval = false;
			}
		
			return retval;
		}

		
        public bool rmove(float x, float y, float z){
            return sendCommand(packCommand("rmove", x,y,z));
        }
		
		public bool move(float x, float y, float z){
			return sendCommand(packCommand("move", x,y,z));
		}
		
		public bool setpos(float x, float y, float z){
			return sendCommand(packCommand("setpos", x, y, z));
		}
		
		public bool pos(ref String s){
			
			// acquire __portReadLock before acquiring __portWriteLock so that 
			// the answer we will receive will be ours and only ours
			lock(__portReadLock){
				// send command to get current position
				if (!sendCommand("pos")) return false;
				
				// get the result
				if(!readResponse(ref s)) return false;
			}
			return true;
		}
		
		public static void CorvusManagerTest(){
			CorvusManager cm = new CorvusManager("COM6", 57600);
            cm.rmove(-6, -5, -2);
            Thread.Sleep(5000);
            cm.rmove(6, 5, 2);
		}
    
		public static void CorvusManagerTestPos(){
			CorvusManager corman = new CorvusManager("COM6", 57600);
			
			Console.WriteLine("Getting position...");
			String s = null;
			
			if (corman.pos(ref s))
				Console.WriteLine("Position successfully read: " + s);
			else
				Console.WriteLine("Impossible to read position.");
			
			Console.WriteLine("Moving Origin to point 3, 2, -1.5");
			
			if (corman.setpos( 3, 2, (float)1.5))
				Console.WriteLine("Origin Successfully moved!");
			else
				Console.WriteLine("Faled to move origin!");
			
			Console.WriteLine("Getting new position.");
			if (corman.pos(ref s))
				Console.WriteLine("New position is " + s);
			else
				Console.WriteLine("Impossible to read new position.");
			
			Console.WriteLine("Moving to origin");
			if (!corman.move(0,0,0))
				Console.WriteLine("Impossible to move to origin");
			
			corman.Close();
		}
	}
}
