﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Utils
{
    public class Output
    {
        public static void WriteToDebugOrConsole(String message)
        {
            if(System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debug.Write("\n" + message + "\n");
                Console.WriteLine("\n" + message + "\n");
            }
            else
            {
                Console.WriteLine("\n" + message + "\n");
            }
        }

        public static void Debug(String tag, String message)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debug.Write("\n" + tag + " : " + message);
            }
        }
    }

}