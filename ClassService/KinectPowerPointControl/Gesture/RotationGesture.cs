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
        public int State { get; set; }

        private double initialRightHeight = 0;
        private double initialLeftHeight = 0;

        private double heightDelta = 0.05;

        public RotationGesture()
        {
            State = 0;
        }

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            ISkeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.HandRight;
            var leftHand = skeleton.HandLeft;
            var spine = skeleton.Spine;
            var rightShoulder = skeleton.ShoulderRight;
            var leftShoulder = skeleton.ShoulderLeft;

            if( 
                rightHand.Position.Y <= spine.Position.Y
                || leftHand.Position.Y <= spine.Position.Y 
                )
            {
                State = 0;
                return false;
            }

            //Hands should be above spine
            if (
                isClose(rightHand.Position.Y, leftHand.Position.Y, handsYDistanceToStart)
                )
            {
                if (State != 1)
                    Output.Debug("RotationGesture", "hand at same height");
                //Gesture was started. Hands are at the same height
                initialRightHeight = rightHand.Position.Y;
                initialLeftHeight = leftHand.Position.Y;
                State = 1;
                return false;
            }

            if (State == 1)
            {
                //Output.Debug("RotationGesture", "initial right " + initialRightHeight.ToString() + " " + rightHand.Position.Y);
                //Output.Debug("RotationGesture", "initial left " + initialLeftHeight.ToString() + " " + leftHand.Position.Y);

                //Right hand is above left
                if (rightHand.Position.Y > initialRightHeight + heightDelta && leftHand.Position.Y < initialLeftHeight - heightDelta)
                {
                    Name = GestureEvents.ROTATE_LEFT;
                    State = 2;
                    Output.Debug("RotationGesture", "Rotate Left " + initialRightHeight.ToString() + " " + initialLeftHeight.ToString());
                    return true;
                }
                else
                    if (rightHand.Position.Y < initialRightHeight - heightDelta && leftHand.Position.Y > initialLeftHeight + heightDelta)
                    {
                        Name = GestureEvents.ROTATE_RIGHT;
                        State = 2;
                        Output.Debug("RotationGesture", "Rotate Right " + initialRightHeight.ToString() + " " + initialLeftHeight.ToString());
                        return true;
                    }

                return false;
            }

            State = 0;
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
