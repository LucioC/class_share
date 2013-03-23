using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Windows.Threading;

namespace KinectPowerPointControl
{
    public class KinectWindowControl
    {
        delegate void CloseDelegate();
        private Thread thread;
        private Window window;

        public KinectWindowControl()
        {

        }

        public void RunWindowInNewThread()
        {
            thread = new Thread(this.StartWindow);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        public void StopThread()
        {
            if (window.Dispatcher.CheckAccess())
                window.Close();
            else
                window.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(window.Close));
        }

        [STAThread]
        public void StartWindow()
        {
            window = new MainWindow();
            window.Show();
            System.Windows.Threading.Dispatcher.Run();
        }
    }
}
