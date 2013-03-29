using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace KinectPowerPointControl.Gesture
{
    public interface IGestureRecognizer
    {
        /// <summary>
        /// Name of the gesture that was recognized, after identify gesture returned true
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Time interval in miliseconds between triggering the identified gesture event a second time
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
