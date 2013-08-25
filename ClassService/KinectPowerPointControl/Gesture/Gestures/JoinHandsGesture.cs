using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore.Utils;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    public class JoinHandsGesture: IGestureRecognizer
    {
        private IntervalControl openJoinHandsIntervalControl = new IntervalControl();

        public JoinHandsGesture()
        {
            Name = GestureEvents.JOIN_HANDS;
            openJoinHandsIntervalControl.Interval = 1500;
            State = 0;
        }

        public int State { get; set; }

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            ISkeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.HandRight;
            var leftHand = skeleton.HandLeft;
            var spine = skeleton.Spine;

            if (rightHand.Position.Y < spine.Position.Y && leftHand.Position.Y < spine.Position.Y)
            {
                State = 0;
                return false;
            }
            else if(rightHand.Position.Z > spine.Position.Z - 0.1 || leftHand.Position.Z > spine.Position.Z - 0.1)
            {
                State = 0;
                return false;
            }

            //Gesture already executed
            if (State == 2) return false;

            float handsDifferenceX = rightHand.Position.X - leftHand.Position.X;
            float handsDifferenceY = rightHand.Position.Y - leftHand.Position.Y;

            //separate hands
            if(State == 0)
            if (Math.Abs(handsDifferenceX) > 0.5 )
            {
                //Gesture started
                State = 1;
                Output.Debug("JointHandsGesture", "Hands are separated");

                openJoinHandsIntervalControl.TriggerIt();
                return false;
            }

            //join hands after separated
            if( State == 1 )
            if (Math.Abs(handsDifferenceX) <= 0.1 && Math.Abs(handsDifferenceY) <= 0.1)
            {
                //If took to long, is not a join hands gesture
                if (openJoinHandsIntervalControl.HasIntervalPassed())
                {
                    State = 2;
                    return false;
                }

                if (userState.IsRightHandGripped || userState.IsLeftHandGripped)
                {
                    State = 2;
                    return false;
                }

                //Gesture executed
                State = 2;
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
