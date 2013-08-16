using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore.Utils;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    public class SwipeRightGesture: IGestureRecognizer
    {
        public SwipeRightGesture()
        {
            Name = GestureEvents.SWIPE_RIGHT;
            State = 0;
        }

        public int State { get; set; }
        SkeletonPoint initialPosition;

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            ISkeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.HandRight;
            var leftHand = skeleton.HandLeft;
            var head = skeleton.Head;
            var centerShoulder = skeleton.ShoulderCenter;

            if(IsBelowMinimumHeight(rightHand,centerShoulder))
            {
                if (State == 1 && IsAtRight(rightHand.Position))
                {
                    State = 0;
                    return true;
                }

                State = 0;
                return false;
            }

            //If hand is above minimun height and is closed then advance to next state
            if (State == 0 && userState.IsRightHandGripped)
            {
                State = 1;
                initialPosition = rightHand.Position;

                Output.Debug("SwipeRight", "Identifyed hand above shoudler");

                return false;
            }

            //If hands was swipe to the right
            if (State == 1)
            {
                SkeletonPoint nextPoint = rightHand.Position;

                if ((IsAtRight(nextPoint) && !userState.IsRightHandGripped))
                {
                    State = 0;
                    Output.Debug("SwipeRight", "Right Gesture Executed");
                    return true;
                }
                else if (!userState.IsRightHandGripped)
                {
                    Output.Debug("SwipeRight", "Hand Open and not moved to gesture, reseting state");
                    State = 0;
                    return false;
                }

                return false;
            }

            State = 0;
            return false;
        }

        private bool IsAtRight(SkeletonPoint nextPoint)
        {
            return nextPoint.X > initialPosition.X + 0.15;
        }

        private static bool IsBelowMinimumHeight(Joint rightHand, Joint centerShoulder)
        {
            return rightHand.Position.Y < centerShoulder.Position.Y - 0.2;
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
