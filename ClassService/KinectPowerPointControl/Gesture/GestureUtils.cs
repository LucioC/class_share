using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    /***
     * Lezz Z is closer to kinect device
     * Less Y is down of the vision of kinect
     * More X is more to the Left side vision of kinect, and more to the Right side of the user if facing forward to the device
     * **/
    public class GestureUtils
    {
        public static float DistanceXYBetweenPoints(SkeletonPoint point1, SkeletonPoint point2)
        {
            double d1 = point1.X - point2.X;
            double d2 = point1.Y - point2.Y;

            return (float)Math.Sqrt(d1 * d1 + d2 * d2);
        }

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
            var spine = skeleton.Joints[JointType.Spine];
            var hipCenter = skeleton.Joints[JointType.HipCenter];

            if (shoulderRight.TrackingState != JointTrackingState.Tracked || shoulderLeft.TrackingState != JointTrackingState.Tracked
                || (spine.TrackingState != JointTrackingState.Tracked && hipCenter.TrackingState != JointTrackingState.Tracked))
            {
                isFacingForward = false;
            }
            else // If user is facing forward
            {
                //Gets one of the center points that is being tracked
                var center = (spine.TrackingState != JointTrackingState.Tracked) ? hipCenter : spine;

                isFacingForward = IsUserFacingForward(new KinectJointWrapper(shoulderRight), new KinectJointWrapper(shoulderLeft), new KinectJointWrapper(center));
            }

            return isFacingForward;
        }

        public static bool IsUserFacingForward(IJoint shoulderRight, IJoint shoulderLeft, IJoint center)
        {
            float angleOfUserCenterTillKinectDevice = GetAngleOfLineBetweenTwoPoints(new SkeletonPoint(), center.Position);
            float angleOfUserShoulderLine = GetAngleOfLineBetweenTwoPoints(shoulderLeft.Position, shoulderRight.Position);

            float angleDifference = angleOfUserCenterTillKinectDevice - angleOfUserShoulderLine;
            angleDifference -= 90f;

            if (Math.Abs(angleDifference) > 10)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /** * Determines the angle of a straight line drawn between point one and two. 
         * The number returned, which is a float in degrees, tells us how much we have to rotate a horizontal line clockwise for it to match the line 
         * between the two points. 
         * If you prefer to deal with angles using radians instead of degrees, just change the last line to: "return Math.Atan2(yDiff, xDiff);" 
         */
        public static float GetAngleOfLineBetweenTwoPoints(SkeletonPoint p1, SkeletonPoint p2) 
        { 
            float xDiff = p2.X - p1.X; 
            float zDiff = p2.Z - p1.Z; 
            return (float)(Math.Atan2(zDiff, xDiff) * (180 / Math.PI)); 
        } //- See more at: http://wikicode.wikidot.com/get-angle-of-line-between-two-points#sthash.kjEbIDbN.dpuf

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

        public static float AngleBetweenPoints(SkeletonPoint p1, SkeletonPoint p2)
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
