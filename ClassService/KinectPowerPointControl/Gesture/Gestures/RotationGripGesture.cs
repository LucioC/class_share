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
        public float MinimumYDistanceToStartRotation = 0.12f;

        public float CurrentAngleDelta { get; set; }

        public RotationGripGesture()
        {
            State = 0;
        }

        public bool IdentifyRotationGesture(IJoint rightHand, IJoint leftHand, IJoint spine, bool isLeftHandGripped, bool isRightHandGripped)
        {
            if (
                GestureUtils.AreHandsBelowSpine(rightHand,leftHand,spine)
                )
            {
                State = 0;
                CurrentAngleDelta = 0;
                return false;
            }

            var handInFront = ( rightHand.Position.Z > leftHand.Position.Z ) ? rightHand : leftHand;
            float halfDistanceBetweenHandAndSpine = GestureUtils.HalfDistanceBetweenHandAndSpine(handInFront, spine);
            if(GestureUtils.AreHandsSeparatedInZ(rightHand,leftHand, halfDistanceBetweenHandAndSpine))
            {
                State = 0;
                CurrentAngleDelta = 0;
                return false;
            }

            if(State == 0)
            {
                if(!GestureUtils.AreHandsSeparatedInY(rightHand,leftHand, MinimumYDistanceToStartRotation) )
                {
                    return false;
                }
            }

            if (
                isLeftHandGripped && isRightHandGripped && State == 0
                )
            {
                initialRightHand = rightHand.Position;
                initialLeftHand = leftHand.Position;
                initialAngle = GestureUtils.AngleBetweenPoints(initialLeftHand, initialRightHand);
                CurrentAngleDelta = 0;
                State = 1;
                Output.Debug("RotationGesture","Initial position");
                return false;
            }
            else if ( !isLeftHandGripped || !isRightHandGripped)
            {
                State = 0;
                CurrentAngleDelta = 0;
                return false;
            }

            if (State == 1)
            {
                bool rightWasUp = (initialRightHand.Y > initialLeftHand.Y) ? true : false;
                bool isRightUpNow = (rightHand.Position.Y > leftHand.Position.Y) ? true : false;

                float currentAngle = GestureUtils.AngleBetweenPoints(leftHand.Position, rightHand.Position);
                CurrentAngleDelta = initialAngle - currentAngle;
                
                //If hands were significantly distant and changed heights then rotation happened
                if (rightWasUp != isRightUpNow)
                {
                    Output.Debug("RotationGesture", "One hand above other. Right hand was up ? " + rightWasUp);
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
