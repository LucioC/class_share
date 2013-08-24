//------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.ControlsBasics
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App(): base()
        {
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DesktopMainWindow window = new DesktopMainWindow();
            Application.Current.MainWindow = window;
            this.MainWindow.Show();
        }
    }
}
