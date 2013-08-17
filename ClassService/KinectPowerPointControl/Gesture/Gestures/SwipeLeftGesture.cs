using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore.Utils;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    public class SwipeLeftGesture: IGestureRecognizer
    {
        public SwipeLeftGesture()
        {
            Name = GestureEvents.SWIPE_LEFT;
            State = 0;
        }

        public int State { get; set; }
        private SkeletonPoint initialPosition;

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            ISkeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.HandRight;
            var leftHand = skeleton.HandLeft;
            var head = skeleton.Head;
            var centerShoulder = skeleton.ShoulderCenter;

            if (IsBelowMinimumHeight(rightHand, centerShoulder))
            {
                if (State == 1 && IsAtLeft(rightHand.Position))
                {
                    State = 0;
                    return true;
                }

                State = 0;
                return false;
            }

            if (State == 0 && userState.IsRightHandGripped )
            {
                State = 1;
                initialPosition = rightHand.Position;

                Output.Debug("SwipeLeft", "Identifyed hand above shoudler");

                return false;
            }

            if (State == 1)
            {
                SkeletonPoint nextPoint = rightHand.Position;
                
                if ((IsAtLeft(nextPoint) && !userState.IsRightHandGripped))
                {
                    State = 0;
                    Output.Debug("SwipeLeft", "Left Gesture Executed");
                    return true;
                }
                else if (!userState.IsRightHandGripped)
                {
                    Output.Debug("SwipeLeft", "Hand Open and not moved to gesture, reseting state");
                    State = 0;
                    return false;
                }

                return false;
            }

            State = 0;
            return false;
        }

        private static bool IsBelowMinimumHeight(IJoint rightHand, IJoint centerShoulder)
        {
            return rightHand.Position.Y < centerShoulder.Position.Y - 0.1;
        }

        private bool IsAtLeft(SkeletonPoint nextPoint)
        {
            return nextPoint.X < initialPosition.X - 0.15;
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
