using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace KinectPowerPointControl.Gesture
{
    public class Move: IGestureRecognizer
    {
        protected IntervalControl intervalControl;

        public Move()
        {
            intervalControl = new IntervalControl();
            Interval = 500;
        }

        public bool IdentifyGesture(Skeleton skeleton)
        {
            intervalControl.Interval = Interval;

            if (!intervalControl.HasIntervalPassed()) return false;

            var rightHand = skeleton.Joints[JointType.HandRight];
            var leftHand = skeleton.Joints[JointType.HandLeft];
            var spine = skeleton.Joints[JointType.Spine];

            if (rightHand.Position.X > spine.Position.X + 0.45 && rightHand.Position.Y > spine.Position.Y)
            {
                Name = GestureEvents.MOVE_RIGHT;
                intervalControl.TriggerIt();
                return true;
            }

            if (rightHand.Position.X < spine.Position.X - 0.1)
            {
                Name = GestureEvents.MOVE_LEFT;
                intervalControl.TriggerIt();
                return true;
            }

            if (rightHand.Position.Y > spine.Position.Y + 0.3)
            {
                Name = GestureEvents.MOVE_UP;
                intervalControl.TriggerIt();
                return true;
            }

            if (rightHand.Position.Y < spine.Position.Y && rightHand.Position.X > spine.Position.X + 0.45)
            {
                Name = GestureEvents.MOVE_DOWN;
                intervalControl.TriggerIt();
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
