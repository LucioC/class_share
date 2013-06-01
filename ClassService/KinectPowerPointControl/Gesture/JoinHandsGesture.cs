using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore.Utils;

namespace KinectPowerPointControl.Gesture
{
    public class JoinHandsGesture: IGestureRecognizer
    {
        public JoinHandsGesture()
        {
            Name = GestureEvents.JOIN_HANDS;
        }

        int state = 0;

        public bool IdentifyGesture(Skeleton skeleton)
        {
            var rightHand = skeleton.Joints[JointType.HandRight];
            var leftHand = skeleton.Joints[JointType.HandLeft];
            var spine = skeleton.Joints[JointType.Spine];

            if (rightHand.Position.Y < spine.Position.Y && leftHand.Position.Y < spine.Position.Y)
            {
                state = 0;
                return false;
            }

            //Gesture already executed
            if (state == 1) return false;

            float handsDifferenceX = rightHand.Position.X - leftHand.Position.X;
            float handsDifferenceY = rightHand.Position.Y - leftHand.Position.Y;

            if (Math.Abs(handsDifferenceX) <= 0.05 && Math.Abs(handsDifferenceY) <= 0.05)
            {
                //Gesture executed
                state = 1;
                Output.Debug("JoinHandsGesture", "Joins Hand Event");
                return true;
            }

            return false;
        }

        public string Name
        {
            get;
            set;
        }

        public long Interval
        {
            get;
            set;
        }
    }
}
