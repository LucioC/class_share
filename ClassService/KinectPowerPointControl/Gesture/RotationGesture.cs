using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore.Utils;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    public class RotationGesture: IGestureRecognizer
    {
        private float handsYDistanceToStart = 0.03f;
        private int state = 0;

        private double initialRightHeight = 0;
        private double initialLeftHeight = 0;

        private double heightDelta = 0.05;

        public RotationGesture()
        {
        }

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            Skeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.Joints[JointType.HandRight];
            var leftHand = skeleton.Joints[JointType.HandLeft];
            var spine = skeleton.Joints[JointType.Spine];
            var rightShoulder = skeleton.Joints[JointType.ShoulderRight];
            var leftShoulder = skeleton.Joints[JointType.ShoulderLeft];

            if( 
                rightHand.Position.Y <= spine.Position.Y
                || leftHand.Position.Y <= spine.Position.Y 
                )
            {
                state = 0;
                return false;
            }

            //Hands should be above spine
            if (
                isClose(rightHand.Position.Y, leftHand.Position.Y, handsYDistanceToStart)
                )
            {
                if (state != 1)
                    Output.Debug("RotationGesture", "hand at same height");
                //Gesture was started. Hands are at the same height
                initialRightHeight = rightHand.Position.Y;
                initialLeftHeight = leftHand.Position.Y;
                state = 1;
                return false;
            }

            if (state == 1)
            {
                //Output.Debug("RotationGesture", "initial right " + initialRightHeight.ToString() + " " + rightHand.Position.Y);
                //Output.Debug("RotationGesture", "initial left " + initialLeftHeight.ToString() + " " + leftHand.Position.Y);

                //Right hand is above left
                if (rightHand.Position.Y > initialRightHeight + heightDelta && leftHand.Position.Y < initialLeftHeight - heightDelta)
                {
                    Name = GestureEvents.ROTATE_LEFT;
                    state = 2;
                    Output.Debug("RotationGesture", "Rotate Left " + initialRightHeight.ToString() + " " + initialLeftHeight.ToString());
                    return true;
                }
                else
                    if (rightHand.Position.Y < initialRightHeight - heightDelta && leftHand.Position.Y > initialLeftHeight + heightDelta)
                    {
                        Name = GestureEvents.ROTATE_RIGHT;
                        state = 2;
                        Output.Debug("RotationGesture", "Rotate Right " + initialRightHeight.ToString() + " " + initialLeftHeight.ToString());
                        return true;
                    }

                return false;
            }

            state = 0;
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
