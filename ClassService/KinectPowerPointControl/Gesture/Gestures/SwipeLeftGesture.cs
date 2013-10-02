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
        public float minimumDistanceToTrigger = 0.15f;
        public float heightToReset = 0.1f;

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            ISkeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.HandRight;
            var leftHand = skeleton.HandLeft;
            var head = skeleton.Head;
            var spine = skeleton.Spine;

            if (State == 1 && 
                GestureUtils.HasMovedToLeft(initialPosition, rightHand.Position, minimumDistanceToTrigger) && 
                GestureUtils.IsHandBelow(rightHand, initialPosition, heightToReset))
            {
                State = 0;
                return true;
            } 
            else if (GestureUtils.IsHandBelow(rightHand, spine))
            {
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
                
                if ((GestureUtils.HasMovedToLeft(initialPosition, nextPoint, minimumDistanceToTrigger) && !userState.IsRightHandGripped))
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
