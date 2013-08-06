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
        }

        int state = 0;
        SkeletonPoint initialPosition;

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            Skeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.Joints[JointType.HandRight];
            var leftHand = skeleton.Joints[JointType.HandLeft];
            var head = skeleton.Joints[JointType.Head];
            var centerShoulder = skeleton.Joints[JointType.ShoulderCenter];

            //If hand is above minimun height and is closed then advance to next state
            if (state == 0 && userState.IsRightHandGripped && !IsBelowMinimumHeight(rightHand, centerShoulder))
            {
                state = 1;
                initialPosition = rightHand.Position;

                Output.Debug("SwipeRight", "Identifyed hand above shoudler");

                return false;
            }

            //If hands was swipe to the right
            if (state == 1)
            {
                SkeletonPoint nextPoint = rightHand.Position;

                //If hand went left then reset state
                if (nextPoint.X < initialPosition.X - 0.1)
                {
                    Output.Debug("SwipeRight", "Went Left, reset");
                    state = 0;
                    return false;
                }

                if ((IsAtRight(nextPoint) && !userState.IsRightHandGripped) || (IsAtRight(nextPoint) && IsBelowMinimumHeight(rightHand, centerShoulder)))
                {
                    state = 0;
                    Output.Debug("SwipeRight", "Right Gesture Executed");
                    return true;
                }
                else if (!userState.IsRightHandGripped)
                {
                    Output.Debug("SwipeRight", "Hand Open and not moved to gesture, reseting state");
                    state = 0;
                    return false;
                }

                return false;
            }

            state = 0;
            return false;
        }

        private bool IsAtRight(SkeletonPoint nextPoint)
        {
            return nextPoint.X > initialPosition.X + 0.2;
        }

        private static bool IsBelowMinimumHeight(Joint rightHand, Joint centerShoulder)
        {
            return rightHand.Position.Y < centerShoulder.Position.Y - 0.1;
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
