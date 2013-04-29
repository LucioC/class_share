using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace KinectPowerPointControl
{
    public class KinectUtils
    {
        public static double DistanceBetweenSkeletonPoints(SkeletonPoint p1, SkeletonPoint p2)
        {
            double distance = 0;

            distance = Math.Pow(p1.X - p2.X, 2)
                        + Math.Pow(p1.Y - p2.Y, 2)
                        + Math.Pow(p1.Z - p2.Z, 2);
            distance = Math.Sqrt(distance);

            return distance;
        }
    }
}
