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
		Object __portWriteLock; // lock variable for writing on the port

        public CorvusManager(string portName, int baudRate) : base(portName, baudRate)
        {
			__portWriteLock = new Object();
        }

        protected void setOpen(){
           if(!IsOpen){
               Open();
           }
        }
		
		public void sendCommand(String venusCommand){
			// prevents multiple thread to write out of the port
			lock(__portWriteLock){
                setOpen();
				WriteLine(venusCommand);
			}
		}
		
		public static string packCommand(String command, params float[] parameters){
			String packedString = "";
			
			for(int i = 0; i < parameters.Length ; ++i){
				packedString += Convert.ToString(parameters[i]) + " ";
			}
			
			return packedString + command ;
		}

        public bool rmove(float x, float y, float z){
            sendCommand(packCommand("rmove", x,y,z));

            return true;
        }
		
		public bool move(float x, float y, float z){
			sendCommand(packCommand("move", x,y,z));
			
			return true;
		}
		
		public static void CorvusManagerTest(){
			CorvusManager cm = new CorvusManager("COM6", 57600);
            cm.rmove(-6, -5, -2);
            Thread.Sleep(5000);
            cm.rmove(6, 5, 2);
		}
    }
}
