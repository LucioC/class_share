using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectPowerPointControl
{
    public class KinectNotFoundException : System.Exception
    {
        public KinectNotFoundException()
            : base("Kinect Sensor Not Found")
        {
            
        }

        public KinectNotFoundException(string message)
            : base(message)
        {

        }

    }
}
