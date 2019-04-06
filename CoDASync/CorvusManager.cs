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
		private Object __portReadLock;

        public CorvusManager(string portName, int baudRate) : base(portName, baudRate)
        {
			__portWriteLock = new Object();
			__portReadLock = new Object();
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
		
		public bool readResponse(ref String s){
			bool retval = true;
			
			// read a line from the port
			lock (__portReadLock){
				try
				{
					setOpen();
					s = ReadLine();
				} catch (Exception e){
					retval = false;
				}
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
			// send command to get current position
			if (!sendCommand("pos")) return false;
			
			// get the result
			if(!readResponse(ref s)) return false;
			
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
