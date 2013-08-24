using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using ServiceCore;
using Microsoft.Samples.Kinect.ControlsBasics;
using System.Windows.Media;
using ServiceCore.Utils;

namespace KinectPowerPointControl
{
    public class DesktopMainWindowControl : DefaultCommunicator, IKinectMainWindowControl
    {
        delegate void CloseDelegate();
        public string FilesFolder { get; set; }
        private Thread thread = null;
        private DesktopMainWindow window = null;

        public DesktopMainWindowControl()
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
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        private void StartWindow()
        {
            Microsoft.Samples.Kinect.ControlsBasics.App app = new Microsoft.Samples.Kinect.ControlsBasics.App();
            app.InitializeComponent();
            //Listen to the startup event
            app.Startup += this.WindowStartup;
            app.Run();
        }

        private void WindowStartup( Object sender,  StartupEventArgs e)
        {
            //Get the main windows after created on startup
            this.window = (DesktopMainWindow)((App)sender).MainWindow;
            this.window.LoadFilesFromFolder(FilesFolder);
            this.window.MessageSent += this.ReceiveMessage;
        }

        public virtual void ReceiveMessage(string message)
        {
            Output.Debug("Control", message);
            SendMessage(message);
        }

        public bool IsThreadRunning()
        {
            if (thread == null || window == null || !thread.IsAlive) return false;
            return true;
        }
    }
}
