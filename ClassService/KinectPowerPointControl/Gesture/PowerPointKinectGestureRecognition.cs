using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using CommonUtils;
using System.Collections;
using System.Collections.Specialized;

namespace KinectPowerPointControl.Gesture
{
    public class PowerPointKinectGestureRecognition: AbstractKinectGestureRecognition
    {
        public PowerPointKinectGestureRecognition(): base()
        {
            gestureRecognizers.Add(new SwipeRightGesture());
            gestureRecognizers.Add(new SwipeLeftGesture());
            gestureRecognizers.Add(new JoinHandsGesture());
            gestureRecognizers.Add(new OutstretchedArmMOvementGesture());
        }

    }
}
