using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    public class Output
    {
        public void WriteToDebugAndConsole(String message)
        {
            System.Diagnostics.Debug.Write("\n" + message + "\n");
            //Console.WriteLine("\n" + message + "\n");
        }
    }

}
