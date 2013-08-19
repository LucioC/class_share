using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore;

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

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            ISkeleton skeleton = userState.Skeleton;

            //Should be in grip mode only for left hand
            if (!userState.IsRightHandGripped)
            {
                State = 0;
                return false;
            }
            if (userState.IsLeftHandGripped)
            {
                State = 0;
                return false;
            }

            var rightHand = skeleton.HandRight;
            if (State == 0)
            {
                lastHandPosition = rightHand.Position;
                State = 1;
            }

            var leftHand = skeleton.HandLeft;
            var spine = skeleton.Spine;

            SkeletonPoint currentPoint = rightHand.Position;

            float xd = currentPoint.X - lastHandPosition.X;

            if (xd > 0 && xd > DistanceToTriggerMove)
            {
                Name = GestureEvents.MOVE_LEFT;
                lastHandPosition.X += DistanceToTriggerMove;
                return true;
            }
            if (xd < 0 && Math.Abs(xd) > DistanceToTriggerMove)
            {
                Name = GestureEvents.MOVE_RIGHT;
                lastHandPosition.X -= DistanceToTriggerMove;
                return true;
            }
                        
            float xy = currentPoint.Y - lastHandPosition.Y;
            if (xy > 0 && xy > DistanceToTriggerMove)
            {
                Name = GestureEvents.MOVE_DOWN;
                lastHandPosition.Y += DistanceToTriggerMove;
                return true;
            }
            if (xy < 0 && Math.Abs(xy) < DistanceToTriggerMove)
            {
                Name = GestureEvents.MOVE_UP;
                lastHandPosition.Y -= DistanceToTriggerMove;
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
