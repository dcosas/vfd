using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace HelloWorld
{
    /// <summary>
    /// This is a simple Hello World program for a Matrix Orbital display.  Note that System.IO.Ports is used to access the serial port.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main will preform all operations for this program.  No parameters are required and no variables are returned.
        /// </summary>
        /// <param name="args"></param>
        string getVFDPortName()
        {
            string[] ports = SerialPort.GetPortNames();
            string vfdPort = String.Empty;
      
            SerialPort port = new SerialPort();
            port.BaudRate = 19200;      //Set the baud rate to display default, 19200
            port.DataBits = 8;      //Display uses 8N1 serial protocol
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;
            port.ReadTimeout = 100;
            //define read version command
            char[] response = { '0' };
            byte[] cmnd_version = { 254, 54 };

            foreach(string currentPort in ports)
            {
                port.PortName = currentPort;
                port.Open();    //Open port
                port.Write(cmnd_version, 0, cmnd_version.Length);
                try
                {
                    port.ReadTimeout = 100;
                    port.Read(response, 0, 1);
                }
                catch(Exception error)
                {

                }
                if (response[0] == 'a')
                {
                    Console.WriteLine("Found VFD on " + currentPort);
                    vfdPort = currentPort;
                    port.Close();
                    break;
                }
                port.Close();             
            }
            
            return vfdPort;
        }

        /// <summary>
        /// Main will preform all operations for this program.  No parameters are required and no variables are returned.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Program vfdObj = new Program();
            

            //Create a serial port object
            SerialPort port = new SerialPort();
            port.PortName = vfdObj.getVFDPortName(); //Provide the name of the port to which the display is attached
            port.BaudRate = 19200;      //Set the baud rate to display default, 19200
            port.DataBits = 8;      //Display uses 8N1 serial protocol
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;

            //Send port information to console screen
            Console.WriteLine("Port selected: " + port.PortName);
            Console.WriteLine("Baud Rate: " + port.BaudRate);
            Console.WriteLine("Press ENTER to clear screen for message.");
            Console.ReadLine();

            try
            {
                port.Open();    //Open port
                //define read version command
               
                //Define clear screen command
                byte[] clear_screen = new byte[2];
                clear_screen[0] = 254;
                clear_screen[1] = 88;
                //Send clear screen command to display
                port.Write(clear_screen, 0, clear_screen.Length);

                //Create a string to send to display
                string message = "Hello World!";
                message = args[0];
                //String must be encoded as ASCII bytes, create an enconding variable
                System.Text.ASCIIEncoding encoding = new ASCIIEncoding();

                Console.WriteLine("Message reads: " + message);

                //Write ASCII encoded string to the display
                Console.WriteLine("Press ENTER to send message to port.");
                Console.ReadLine();
                port.Write(encoding.GetBytes(message), 0, encoding.GetByteCount(message));

                Console.WriteLine("Message sent, press ENTER to quit.");
                Console.ReadLine();

                port.Close();   //Close port
            }

            //catch and report any errors in serial communication
            catch (Exception error)
            {
                Console.WriteLine("Error in communication with {0}!", port.PortName);
            }
        }
    }
}
