using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace KinectPowerPointControl.Gesture
{
    public class RotationGesture: IGestureRecognizer
    {
        protected IntervalControl intervalControl;

        private bool started = false;
        private float handsYDistanceToStart = 0.03f;

        public RotationGesture()
        {
            intervalControl = new IntervalControl();
            Interval = 300;
        }

        public bool IdentifyGesture(Skeleton skeleton)
        {
            intervalControl.Interval = Interval;

            if (!intervalControl.HasIntervalPassed()) return false;

            var rightHand = skeleton.Joints[JointType.HandRight];
            var leftHand = skeleton.Joints[JointType.HandLeft];
            var spine = skeleton.Joints[JointType.Spine];
            var rightShoulder = skeleton.Joints[JointType.ShoulderRight];
            var leftShoulder = skeleton.Joints[JointType.ShoulderLeft];

            //Hands should be above spine
            if (rightHand.Position.Y < spine.Position.Y 
                || leftHand.Position.Y < spine.Position.Y
                || rightHand.Position.X > spine.Position.X + 0.25
                || leftHand.Position.X < spine.Position.X - 0.25)
            {
                started = false;
                return false;
            }
            else //Verify if hands are at the same height to start and are not so apart
            if (!started 
                && !isClose(rightHand.Position.Y, leftHand.Position.Y, handsYDistanceToStart) 
                )
            {
                started = false;
                return false;
            }

            if (!started )
            {
                //Gesture was started. Hands are at the same height
                started = true;
                return false;
            }

            //If got here is because gesture was started, now just verify when one hand is above the other

            //Is getting weird results with this, comment for now
          /*  float newDistanceX = GestureUtils.calculateDistanceX(rightHand.Position, leftHand.Position);
            if (newDistanceX > 0.1 )
            {
                return false;
            } */

            //Right hand is above left
            if (rightHand.Position.Y > leftHand.Position.Y + 0.1)
            {
                Name = GestureEvents.ROTATE_LEFT;
                intervalControl.TriggerIt();
                started = false;
                return true;
            }
            else
            if (leftHand.Position.Y > rightHand.Position.Y + 0.1)
            {
                Name = GestureEvents.ROTATE_RIGHT;
                intervalControl.TriggerIt();
                started = false;
                return true;
            }

            return false;
        }

        private bool isClose(float value1, float value2, float acceptedError)
        {
            float difference = value1 - value2;
            difference = Math.Abs(difference);

            if (difference > acceptedError) return false;

            return true;
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
