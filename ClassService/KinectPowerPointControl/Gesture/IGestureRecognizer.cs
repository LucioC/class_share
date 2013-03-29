using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace KinectPowerPointControl.Gesture
{
    public interface IGestureRecognizer
    {
        String Name { get; set; }

        /// <summary>
        /// Time interval in miliseconds that the between triggering the identified gesture event
        /// </summary>
        long Interval { get; set; }

        /// <summary>
        /// Return true if the gesture was identified and an result should be trigger. 
        /// It should be called each frame but may not return true again if the maximum time Interval desired has not passed.
        /// </summary>
        /// <param name="skeleton"></param>
        /// <returns></returns>
        bool IdentifyGesture(Skeleton skeleton);
    }
}
