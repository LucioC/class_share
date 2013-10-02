using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    public class GestureUtils
    {
        //Calculate distance of X axis
        public static float CalculateDistanceX(SkeletonPoint rightHandPosition, SkeletonPoint leftHandPosition)
        {
            float distance = Math.Abs(rightHandPosition.X - leftHandPosition.X);
            return distance;
        }

        public static bool AreHandsBelowSpine(IJoint rightHand, IJoint leftHand, IJoint spine)
        {
            return rightHand.Position.Y < spine.Position.Y && leftHand.Position.Y < spine.Position.Y;
        }

        public static bool HasMovedToRight(SkeletonPoint initialPoint, SkeletonPoint currentPoint, float minimumDistance)
        {
            return currentPoint.X > initialPoint.X + minimumDistance;
        }

        public static bool HasMovedToLeft(SkeletonPoint initialPoint, SkeletonPoint currentPoint, float minimumDistance)
        {
            return currentPoint.X < initialPoint.X - minimumDistance;
        }

        public static bool IsOneHandBelowSpine(IJoint rightHand, IJoint leftHand, IJoint spine)
        {
            return rightHand.Position.Y < spine.Position.Y || leftHand.Position.Y < spine.Position.Y;
        }

        public static bool IsHandBelow(IJoint rightHand, IJoint otherJoint)
        {
            return rightHand.Position.Y < otherJoint.Position.Y;
        }

        public static bool IsHandBelow(IJoint rightHand, SkeletonPoint aPoint, float extra)
        {
            return rightHand.Position.Y < aPoint.Y - extra;
        }

        public static bool AreHandsCloserThan(float handsDifferenceX, float handsDifferenceY, float distance)
        {
            return Math.Abs(handsDifferenceX) <= distance && Math.Abs(handsDifferenceY) <= distance;
        }

        public static bool AreHandsSeparatedInZ(IJoint rightHand, IJoint leftHand, float distance)
        {
            return Math.Abs(GestureUtils.CalculateDistanceZ(rightHand.Position, leftHand.Position)) > distance;
        }

        public static bool IsHandCloseToSpineInZ(IJoint hand, IJoint spine, float minimumDistance)
        {
            return hand.Position.Z > spine.Position.Z - minimumDistance;
        }

        public static bool IsLeftHandCloseToRightOrInFront(IJoint rightHand, IJoint leftHand, float minimumDistanceAllowed)
        {
            //Less Z is closer to sensor
            return leftHand.Position.Z < rightHand.Position.Z || leftHand.Position.Z < rightHand.Position.Z - minimumDistanceAllowed;
        }

        public static float HalfDistanceBetweenRightHandAndSpine(IJoint rightHand, IJoint spine)
        {
            float allowedDistance = (rightHand.Position.Z - spine.Position.Z) / 2;
            return allowedDistance;
        }

        public static bool IsUserFacingForward(Skeleton skeleton)
        {
            bool isFacingForward = false;

            var shoulderRight = skeleton.Joints[JointType.ShoulderRight];
            var shoulderLeft = skeleton.Joints[JointType.ShoulderLeft];

            if (shoulderRight.TrackingState != JointTrackingState.Tracked || shoulderLeft.TrackingState != JointTrackingState.Tracked)
            {
                isFacingForward = false;
            }
            else // If user is facing forward
                if (shoulderRight.Position.X > shoulderLeft.Position.X && GestureUtils.CalculateDistanceZ(shoulderRight.Position, shoulderLeft.Position) < 0.1)
                {
                    isFacingForward = true;
                }
                else
                {
                    isFacingForward = false;
                }

            return isFacingForward;
        }

        public static float CalculateDistanceZ(SkeletonPoint rightHandPosition, SkeletonPoint leftHandPosition)
        {
            float distance = Math.Abs(rightHandPosition.Z- leftHandPosition.Z);
            return distance;
        }

        public static float CalculateDistanceY(SkeletonPoint rightHandPosition, SkeletonPoint leftHandPosition)
        {
            float distance = Math.Abs(rightHandPosition.Y - leftHandPosition.Y);
            return distance;
        }

        public static float angleBetweenPoints(SkeletonPoint p1, SkeletonPoint p2)
        {
            float deltaY = p2.Y - p1.Y;
            float deltaX = p2.X - p1.X;

            float angleInDegrees = (float)(Math.Atan2(deltaY, deltaX) * 180 / Math.PI);

            return angleInDegrees;
        }

        //From diferent frames an error may occur, eliminate error perception
        public static float NormalizeDistance(float deltaDistance, float errorExpected)
        {
            float result = deltaDistance;

            if (deltaDistance > 0)
            {
                result = deltaDistance - errorExpected;
                if (result < 0) result = 0f;
            }
            else if (deltaDistance < 0)
            {
                result = deltaDistance + errorExpected;
                if (result > 0) result = 0f;
            }

            return result;
        }
    }
}
