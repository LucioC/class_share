using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using ServiceCore;
using ServiceCore.Utils;

namespace KinectPowerPointControl
{
    public class KinectWindowControl : IKinectService
    {
        delegate void CloseDelegate();
        private PRESENTATION_MODE mode = PRESENTATION_MODE.POWERPOINT;
        private Thread thread = null;
        private KinectControlWindow window = null;

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

        public CommandExecutor Executor { get; set; }

        [STAThread]
        public void StartWindow()
        {
            window = new KinectControlWindow(mode, Executor);
            window.Show();
            System.Windows.Threading.Dispatcher.Run();
        }

        public bool IsThreadRunning()
        {
            if (thread == null || window == null || !thread.IsAlive) return false;
            return true;
        }

        public void setMode(PRESENTATION_MODE mode)
        {
            this.mode = mode;
        }
    }
}
