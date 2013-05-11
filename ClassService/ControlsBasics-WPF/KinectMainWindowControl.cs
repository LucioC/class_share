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

namespace KinectPowerPointControl
{
    public class KinectMainWindowControl : DefaultCommunicator, IKinectService
    {
        delegate void CloseDelegate();
        private PRESENTATION_MODE mode = PRESENTATION_MODE.POWERPOINT;
        private Thread thread = null;
        private Window window = null;

        public KinectMainWindowControl()
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
        public void StartWindow()
        {            
            /*ResourceDictionary resourceDictionary = new ResourceDictionary();// { Source = new Uri(@"Common/StandardStyles.xaml") };
            resourceDictionary.Add("BlueBrush", new SolidColorBrush() { Color = (Color)ColorConverter.ConvertFromString("#FF00BCF2") });
            resourceDictionary.Add("MediumGreyBrush", new SolidColorBrush() { Color = (Color)ColorConverter.ConvertFromString("#ff6e6e6e") });
            app.Resources = resourceDictionary; */

            Microsoft.Samples.Kinect.ControlsBasics.App app = new Microsoft.Samples.Kinect.ControlsBasics.App();
            app.InitializeComponent();
            app.Run();

            /*
            window = new MainWindow();
            window.Show();
            //app.Run(window);
            System.Windows.Threading.Dispatcher.Run(); */
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
