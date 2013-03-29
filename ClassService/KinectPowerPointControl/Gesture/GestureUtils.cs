using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace KinectPowerPointControl.Gesture
{
    public class GestureUtils
    {
        //Calculate distance of X axis
        public static float calculateDistanceX(SkeletonPoint rightHandPosition, SkeletonPoint leftHandPosition)
        {
            float distance = Math.Abs(rightHandPosition.X - leftHandPosition.X);
            return distance;
        }
    }
}
