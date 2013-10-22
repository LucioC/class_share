using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore.Utils;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    public class JoinHandsGesture: IGestureRecognizer
    {
        private IntervalControl openJoinHandsIntervalControl = new IntervalControl();

        public JoinHandsGesture()
        {
            Name = GestureEvents.JOIN_HANDS;
            openJoinHandsIntervalControl.Interval = 1500;
            State = 0;
        }

        public int State { get; set; }

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            ISkeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.HandRight;
            var leftHand = skeleton.HandLeft;
            var spine = skeleton.Spine;
            var rightShoulder = skeleton.ShoulderRight;
            var leftShoulder = skeleton.ShoulderLeft;

            return IdentifyGesture(rightHand, leftHand, userState.IsRightHandGripped, userState.IsLeftHandGripped, spine, rightShoulder, leftShoulder);
        }

        public bool IdentifyGesture(IJoint rightHand, IJoint leftHand, bool isRightHandGripped, bool isLeftHandGripped, IJoint spine, IJoint rightShoulder, IJoint leftShoulder)
        {
            if (GestureUtils.AreHandsBelowSpine(rightHand, leftHand, spine))
            {
                State = 0;
                return false;
            }
            else if (GestureUtils.IsHandCloseToSpineInZ(rightHand, spine, 0.05f) || GestureUtils.IsHandCloseToSpineInZ(leftHand, spine, 0.05f))
            {
                State = 0;
                return false;
            }

            //Gesture already executed or rejected
            if (State == 2) return false;

            float handsDifferenceX = rightHand.Position.X - leftHand.Position.X;
            float handsDifferenceY = rightHand.Position.Y - leftHand.Position.Y;

            //separate hands
            if (State == 0)
            {
                float DistanceBetweenShoulders = GestureUtils.DistanceXYBetweenPoints(rightShoulder.Position, leftShoulder.Position);
                if (AreHandsSeparatedMoreThan(handsDifferenceX, DistanceBetweenShoulders + 0.15f)
                    && !isLeftHandGripped && !isRightHandGripped)
                {
                    //Gesture started
                    State = 1;
                    Output.Debug("JointHandsGesture", "Hands are separated");

                    //separation started
                    openJoinHandsIntervalControl.TriggerIt();
                    return false;
                }
            }

            //join hands after separated
            if (State == 1)
            {
                if (GestureUtils.AreHandsCloserThan(handsDifferenceX, handsDifferenceY, 0.1f))
                {
                    //If took to long to join hands, is not a join hands gesture
                    if (openJoinHandsIntervalControl.HasIntervalPassed())
                    {
                        State = 2;
                        return false;
                    }

                    if (isRightHandGripped && isLeftHandGripped)
                    {
                        State = 2;
                        return false;
                    }

                    //Gesture executed
                    State = 2;
                    Output.Debug("JoinHandsGesture", "Joins Hand Event");
                    return true;
                }
            }

            return false;
        }

        private static bool AreHandsSeparatedMoreThan(float handsXDifference, float distance)
        {
            return Math.Abs(handsXDifference) > distance;
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
