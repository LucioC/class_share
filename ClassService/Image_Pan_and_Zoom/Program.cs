using System;
using System.Windows.Forms;
using System.Threading;
using CommonUtils;
using System.Drawing;
using System.IO;

namespace ImageZoom
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ImageFormControl f = new ImageFormControl(@"../../poneis.jpg");
            //f.InitializeForm();
            f.RunFormInNewThread();
            //f.Stop();
            System.Threading.Thread.Sleep(10000);
            f.StopThread();
            f.InitializeForm();
        }
    }
    
}
