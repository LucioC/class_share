using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore;
using ServiceCore.Utils;

namespace KinectPowerPointControl.Gesture
{
    public class GripHandMove: IGestureRecognizer
    {
        public GripHandMove()
        {
            Interval = 500;
            DistanceToTriggerMove = 0.05f;
        }

        public float DistanceToTriggerMove { get; set; }

        private SkeletonPoint lastHandPosition;

        public int State { get; set; }

        public bool IdentifyGrippedMove(IJoint rightHand, IJoint leftHand, IJoint spine, bool isRightHandGripped)
        {
            if (!isRightHandGripped)
            {
                State = 0;
                return false;
            }
            float halfDistanceBetweenRightHandAndSpine = GestureUtils.HalfDistanceBetweenHandAndSpine(rightHand, spine);

            if (GestureUtils.IsLeftHandCloseToRightOrInFront(rightHand, leftHand, halfDistanceBetweenRightHandAndSpine))
            {
                State = 0;
                return false;
            }

            if (State == 0)
            {
                lastHandPosition = rightHand.Position;
                State = 1;
            }

            SkeletonPoint currentPoint = rightHand.Position;

            //If X grew, than hand moved right, else left
            float xd = currentPoint.X - lastHandPosition.X;
            if (xd > 0 && xd > DistanceToTriggerMove)
            {
                Name = GestureEvents.MOVE_RIGHT;
                lastHandPosition.X += DistanceToTriggerMove;
                return true;
            }
            if (xd < 0 && Math.Abs(xd) > DistanceToTriggerMove)
            {
                Name = GestureEvents.MOVE_LEFT;
                lastHandPosition.X -= DistanceToTriggerMove;
                return true;
            }

            //If Y grew, hand moved up, else down
            float yd = currentPoint.Y - lastHandPosition.Y;
            if (yd > 0 && yd > DistanceToTriggerMove)
            {
                Name = GestureEvents.MOVE_UP;
                lastHandPosition.Y += DistanceToTriggerMove;
                return true;
            }
            if (yd < 0 && Math.Abs(yd) > DistanceToTriggerMove)
            {
                Name = GestureEvents.MOVE_DOWN;
                lastHandPosition.Y -= DistanceToTriggerMove;
                return true;
            }
            return false;
        }

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            ISkeleton skeleton = userState.Skeleton;

            var rightHand = skeleton.HandRight;
            var leftHand = skeleton.HandLeft;
            var spine = skeleton.Spine;

            return IdentifyGrippedMove(rightHand, leftHand, spine, userState.IsRightHandGripped);
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
