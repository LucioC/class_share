using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.Office.Interop.PowerPoint;
using PowerPointPresentation;
using System.IO;
using ImageZoom;
using KinectPowerPointControl;
using ServiceCore;
using ServiceCore.Utils;
using Microsoft.Samples.Kinect.ControlsBasics;

namespace ClassService
{
    /// <summary>
    /// Initialize a WCF service using the default initialization of the Service class.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceWCF : Service
    {
        public ServiceWCF()
        {
            defaultInitializationOfStaticFields();
            prepareService();
        }

        private void defaultInitializationOfStaticFields()
        {
            fileManager = new ServiceFileManager();
            KinectWindow = new KinectWindowControl();
            ImageForm = new ImageFormControl();
            PresentationControl = new PowerPointControl();
            mainWindow = new DesktopMainWindowControl();
            urlManager = new ServiceUrlManager();
        }        
    }
}

