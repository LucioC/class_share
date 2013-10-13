using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Collections;
using System.Collections.Specialized;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    public class PowerPointKinectGestureRecognition: AbstractKinectGestureRecognition
    {
        public PowerPointKinectGestureRecognition(SkeletonStateRepository skeletonRepository)
            : base(skeletonRepository)
        {
            gestureRecognizers.Add(new SwipeRightGesture());
            gestureRecognizers.Add(new SwipeLeftGesture());
            gestureRecognizers.Add(new JoinHandsGesture());
        }

    }
}
