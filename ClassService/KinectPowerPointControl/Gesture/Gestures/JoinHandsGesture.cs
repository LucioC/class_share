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
        }

        int state = 0;

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            Skeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.Joints[JointType.HandRight];
            var leftHand = skeleton.Joints[JointType.HandLeft];
            var spine = skeleton.Joints[JointType.Spine];

            if (rightHand.Position.Y < spine.Position.Y && leftHand.Position.Y < spine.Position.Y)
            {
                state = 0;
                return false;
            }

            //Gesture already executed
            if (state == 2) return false;

            float handsDifferenceX = rightHand.Position.X - leftHand.Position.X;
            float handsDifferenceY = rightHand.Position.Y - leftHand.Position.Y;

            //separate hands
            if(state == 0)
            if (Math.Abs(handsDifferenceX) > 0.5 )
            {
                //Gesture started
                state = 1;
                Output.Debug("JointHandsGesture", "Hands are separated");

                openJoinHandsIntervalControl.TriggerIt();
                return false;
            }

            //join hands after separated
            if( state == 1 )
            if (Math.Abs(handsDifferenceX) <= 0.1 && Math.Abs(handsDifferenceY) <= 0.1)
            {
                //If took to long, is not a join hands gesture
                if (openJoinHandsIntervalControl.HasIntervalPassed())
                {
                    state = 2;
                    return false;
                }

                if (userState.IsRightHandGripped || userState.IsLeftHandGripped)
                {
                    state = 2;
                    return false;
                }

                //Gesture executed
                state = 2;
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
