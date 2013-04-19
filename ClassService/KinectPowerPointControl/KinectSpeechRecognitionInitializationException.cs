using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectPowerPointControl
{
    public class KinectSpeechRecognitionInitializationException: System.Exception
    {
        public KinectSpeechRecognitionInitializationException()
            : base("Kinect Speech Recognition Initialization Error")
        {
            
        }

        public KinectSpeechRecognitionInitializationException(string message)
            : base(message)
        {

        }
    }
}
