using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using ServiceCore;

namespace KinectPowerPointControl
{
    public class KinectWindowControl : IWindowThreadControl
    {
        delegate void CloseDelegate();
        private Thread thread = null;
        private Window window = null;

        public KinectWindowControl()
        {

        }

        public void StartThread()
        {
            thread = new Thread(this.StartWindow);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        public void StopThread()
        {
            if (!IsThreadRunning()) return;
            if (window.Dispatcher.CheckAccess())
            {
                window.Close();
            }
            else
            {
                //FIXME It will always close and terminate thread?
                window.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(window.Close));                
                window = null;
                thread = null;
            }
        }

        [STAThread]
        public void StartWindow()
        {
            window = new MainWindow();
            window.Show();
            System.Windows.Threading.Dispatcher.Run();
        }


        public bool IsThreadRunning()
        {
            if (thread == null || window == null || !thread.IsAlive) return false;
            return false;
        }
    }
}
