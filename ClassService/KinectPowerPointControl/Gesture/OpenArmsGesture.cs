using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore.Utils;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    public class OpenArmsGesture: IGestureRecognizer
    {
        public OpenArmsGesture()
        {
            Name = GestureEvents.OPEN_ARMS;
        }

        int state = 0;

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            Skeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.Joints[JointType.HandRight];
            var leftHand = skeleton.Joints[JointType.HandLeft];
            var spine = skeleton.Joints[JointType.Spine];

            //If hands are to low then reset state
            if (rightHand.Position.Y < spine.Position.Y || leftHand.Position.Y < spine.Position.Y)
            {
                state = 0;
                return false;
            }

            if (state == 2) return false;

            float handsDistance = rightHand.Position.X - leftHand.Position.X;
            handsDistance = Math.Abs(handsDistance);

            if (state == 0)
            {
                //Hands are close, so can start movement
                if ( handsDistance < 0.2)
                {
                    state = 1;
                    Output.Debug("OpenArmsGesture","Hands are close.");
                }
                return false;
            }

            if (state == 1)
            {
                if (handsDistance > 0.8)
                {
                    Output.Debug("OpenArmsGesture", "Arms are open.");
                    state = 2;
                    return true;
                }
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
