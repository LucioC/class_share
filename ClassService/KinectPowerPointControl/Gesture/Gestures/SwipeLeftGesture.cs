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
        }

        private int state = 0;
        private SkeletonPoint initialPosition;

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            Skeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.Joints[JointType.HandRight];
            var leftHand = skeleton.Joints[JointType.HandLeft];
            var head = skeleton.Joints[JointType.Head];
            var centerShoulder = skeleton.Joints[JointType.ShoulderCenter];

            if (IsBelowMinimumHeight(rightHand, centerShoulder))
            {
                if (state == 1 && IsAtLeft(rightHand.Position))
                {
                    state = 0;
                    return true;
                }

                state = 0;
                return false;
            }

            if (state == 0 && userState.IsRightHandGripped )
            {
                state = 1;
                initialPosition = rightHand.Position;

                Output.Debug("SwipeLeft", "Identifyed hand above shoudler");

                return false;
            }

            if (state == 1)
            {
                SkeletonPoint nextPoint = rightHand.Position;
                
                if ((IsAtLeft(nextPoint) && !userState.IsRightHandGripped))
                {
                    state = 0;
                    Output.Debug("SwipeLeft", "Left Gesture Executed");
                    return true;
                }
                else if (!userState.IsRightHandGripped)
                {
                    Output.Debug("SwipeLeft", "Hand Open and not moved to gesture, reseting state");
                    state = 0;
                    return false;
                }

                return false;
            }

            state = 0;
            return false;
        }

        private static bool IsBelowMinimumHeight(Joint rightHand, Joint centerShoulder)
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
