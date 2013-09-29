using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore;
using ServiceCore.Utils;

namespace KinectPowerPointControl.Gesture
{
    public class CloseHandAfterTime: IGestureRecognizer
    {
        private IntervalControl closeHandIntervalControl = new IntervalControl();
        public CloseHandAfterTime()
        {
            Name = GestureEvents.CLOSE_HAND;
            closeHandIntervalControl.Interval = 2500;
            DistanceToTriggerMove = 0.05f;
        }

        public float DistanceToTriggerMove { get; set; }

        private SkeletonPoint lastHandPosition;
        private SkeletonPoint initialHandPosition;

        public int State { get; set; }

        public bool IdentifyCloseHandAfterTime(IJoint rightHand, IJoint leftHand, IJoint spine, IJoint shoulderCenter, bool isRightHandGripped, bool isLeftHandGripped)
        {
            float betweenSpineAndShoulderHeight = spine.Position.Y + (shoulderCenter.Position.Y - spine.Position.Y)/2;

            if (rightHand.Position.Y < betweenSpineAndShoulderHeight || GestureUtils.CalculateDistanceY(rightHand.Position, leftHand.Position) < 0.2 
                || isLeftHandGripped)
            {
                State = 0;
                return false;
            }

            //Keep track of initial position
            if (State == 0 && !isRightHandGripped)
            {
                initialHandPosition = rightHand.Position;
                closeHandIntervalControl.TriggerIt(); 
                State = 1;
                return false;
            }

            if (State == 1)
            {
                lastHandPosition = rightHand.Position;

                //If hand moved, reset state
                if (GestureUtils.CalculateDistanceY(initialHandPosition, lastHandPosition) > 0.05 ||
                    GestureUtils.CalculateDistanceX(initialHandPosition, lastHandPosition) > 0.05)
                {
                    State = 0;
                    return false;
                }

                if( closeHandIntervalControl.HasIntervalPassed() && isRightHandGripped)
                {

                    State = 2;
                    return true;
                }
            }

            return false;
        }

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            ISkeleton skeleton = userState.Skeleton;

            var rightHand = skeleton.HandRight;
            var leftHand = skeleton.HandLeft;
            var spine = skeleton.Spine;
            var shoulderCenter = skeleton.ShoulderCenter;
            
            return IdentifyCloseHandAfterTime(rightHand, leftHand, spine, shoulderCenter, userState.IsRightHandGripped, userState.IsLeftHandGripped);
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
