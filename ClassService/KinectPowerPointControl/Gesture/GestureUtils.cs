﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

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
        public static float normalizeDistance(float deltaDistance, float errorExpected)
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
