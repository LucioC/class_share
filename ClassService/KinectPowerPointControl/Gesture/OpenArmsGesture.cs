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
            State = 0;
        }

        public int State { get; set; }

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            ISkeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.HandRight;
            var leftHand = skeleton.HandLeft;
            var spine = skeleton.Spine;

            //If hands are to low then reset state
            if (rightHand.Position.Y < spine.Position.Y || leftHand.Position.Y < spine.Position.Y)
            {
                State = 0;
                return false;
            }

            if (State == 2) return false;

            float handsDistance = rightHand.Position.X - leftHand.Position.X;
            handsDistance = Math.Abs(handsDistance);

            if (State == 0)
            {
                //Hands are close, so can start movement
                if ( handsDistance < 0.2)
                {
                    State = 1;
                    Output.Debug("OpenArmsGesture","Hands are close.");
                }
                return false;
            }

            if (State == 1)
            {
                if (handsDistance > 0.8)
                {
                    Output.Debug("OpenArmsGesture", "Arms are open.");
                    State = 2;
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
