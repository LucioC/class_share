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

        private SkeletonPoint lastHandPosition { get; set; }

        private int state { get; set; }

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            Skeleton skeleton = userState.Skeleton;

            //Should be in grip mode
            if (!userState.IsRightHandGripped)
            {
                state = 0;
                return false;
            }

            var rightHand = skeleton.Joints[JointType.HandRight];
            if (state == 0)
            {
                lastHandPosition = rightHand.Position;
                state = 1;
            }

            var leftHand = skeleton.Joints[JointType.HandLeft];
            var spine = skeleton.Joints[JointType.Spine];

            SkeletonPoint currentPoint = rightHand.Position;

            float xd = currentPoint.X - lastHandPosition.X;

            if (xd > 0 && xd > DistanceToTriggerMove)
            {
                Name = GestureEvents.MOVE_RIGHT;
                return true;
            }
            if (xd < 0 && Math.Abs(xd) > DistanceToTriggerMove)
            {
                Name = GestureEvents.MOVE_LEFT;
                return true;
            }
                        
            float xy = currentPoint.Y - lastHandPosition.Y;
            if (xy > 0 && xy > DistanceToTriggerMove)
            {
                Name = GestureEvents.MOVE_UP;
                return true;
            }
            if (xy < 0 && Math.Abs(xy) < DistanceToTriggerMove)
            {
                Name = GestureEvents.MOVE_DOWN;
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
