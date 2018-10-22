using System;
using System.IO.Ports;
/*using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;*/

namespace CoDASync
{
    class CorvusManager : SerialPort
    {

        public CorvusManager(string portName, int baudRate) : base(portName, baudRate)
        {
        }

        protected void setOpen(){
           if(!IsOpen){
               Open();
           }
        }

        public bool rmove(float a1, float a2, float a3){
            setOpen();

            string txData = Convert.ToString(a1) + " " + Convert.ToString(a2) + " " + Convert.ToString(a3);
            txData += " rmove";

            WriteLine(txData);

            return true;
        }
    }
}
