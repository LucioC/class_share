using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectPowerPointControl
{
    public class RightGestureRecognizer: IGestureRecognizer
    {
        public RightGestureRecognizer()
        {
            Name = "RightGesture";
            Interval = 50;
        }

        public string Name
        {
            get;
            protected set;
        }

        public long Interval
        {
            get;
            set;
        }

        public bool IdentifyGesture(Microsoft.Kinect.Skeleton skeleton)
        {
            throw new NotImplementedException();
        }
    }
}
