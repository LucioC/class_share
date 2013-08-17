using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore.Utils;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    public class RotationGripGesture : IRotationGripGesture
    {
        public int State { get; set; }

        private SkeletonPoint initialRightHand;
        private SkeletonPoint initialLeftHand;

        private float initialAngle;

        public float MinimumAngleChangeToTriggerRotation = 60;

        public float CurrentAngleDelta { get; set; }

        private double heightDelta = 0.05;

        public RotationGripGesture()
        {
            State = 0;
        }

        public bool IdentifyRotationGesture(IJoint rightHand, IJoint leftHand, IJoint spine, bool isLeftHandGripped, bool isRightHandGripped)
        {

            if (
                rightHand.Position.Y <= spine.Position.Y
                || leftHand.Position.Y <= spine.Position.Y
                )
            {
                State = 0;
                return false;
            }
            if (
                //isClose(rightHand.Position.Y, leftHand.Position.Y, handsYDistanceToStart)
                isLeftHandGripped && isRightHandGripped && State == 0
                )
            {
                initialRightHand = rightHand.Position;
                initialLeftHand = leftHand.Position;
                initialAngle = GestureUtils.angleBetweenPoints(initialLeftHand, initialRightHand);
                State = 1;
                return false;
            }
            else if ( !isLeftHandGripped || !isRightHandGripped)
            {
                State = 0;
                return false;
            }

            if (State == 1)
            {
                //Output.Debug("RotationGesture", "initial right " + initialRightHeight.ToString() + " " + rightHand.Position.Y);
                //Output.Debug("RotationGesture", "initial left " + initialLeftHeight.ToString() + " " + leftHand.Position.Y);

                bool rightWasUp = (initialRightHand.Y > initialLeftHand.Y) ? true : false;
                bool isRightUpNow = (rightHand.Position.Y > leftHand.Position.Y) ? true : false;

                float currentAngle = GestureUtils.angleBetweenPoints(leftHand.Position, rightHand.Position);
                CurrentAngleDelta = initialAngle - currentAngle;

                if (rightWasUp != isRightUpNow)
                {
                    if (rightWasUp)
                    {
                        Name = GestureEvents.ROTATE_RIGHT;
                        State = 2;
                        return true;
                    }
                    else
                    {
                        Name = GestureEvents.ROTATE_LEFT;
                        State = 2;
                        return true;
                    }
                }
                else
                {
                    if (Math.Abs(CurrentAngleDelta) > MinimumAngleChangeToTriggerRotation)
                    {
                        if (rightWasUp)
                        {
                            if (CurrentAngleDelta > 0)
                            {
                                Name = GestureEvents.ROTATE_RIGHT;
                                State = 2;
                                return true;
                            }
                            else
                            {
                                Name = GestureEvents.ROTATE_LEFT;
                                State = 2;
                                return true;
                            }
                        }
                    }

                }

                return false;
            }

            State = 0;
            return false;
        }

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            ISkeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.HandRight;
            var leftHand = skeleton.HandLeft;
            var spine = skeleton.Spine;
            var rightShoulder = skeleton.ShoulderRight;
            var leftShoulder = skeleton.ShoulderLeft;

            return IdentifyRotationGesture(rightHand, leftHand, spine, userState.IsLeftHandGripped, userState.IsRightHandGripped);
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
