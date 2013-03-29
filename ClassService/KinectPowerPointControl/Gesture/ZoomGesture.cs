using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using CommonUtils;

namespace KinectPowerPointControl.Gesture
{
    public class ZoomGesture
    {
        private long lastZoomTrigged = 0;
        private System.Object lockInterval = new System.Object();

        private Boolean ZoomStarted = false;
        public float HandsDistance { get; set; }
        public float HandsDistanceErrorIgnored { get; set; }
        public long ZoomIntervalMiliseconds { get; set; }

        public String LastEventName = null;

        public ZoomGesture()
        {
            HandsDistance = 0f;
            HandsDistanceErrorIgnored = 0.05f;
            ZoomIntervalMiliseconds = 200;
        }

        public Boolean ProcessZoomGestures(Skeleton skeleton)
        {
            var rightHand = skeleton.Joints[JointType.HandRight];
            var leftHand = skeleton.Joints[JointType.HandLeft];
            var head = skeleton.Joints[JointType.Head];

            var shoulderCenter = skeleton.Joints[JointType.ShoulderCenter];

            //if not tracked then go away
            if (shoulderCenter.TrackingState != JointTrackingState.Tracked)
            {
                ZoomStarted = false;
                return false;
            }
            else //If hands are below shouldcenter dont track gesture 
                if (rightHand.Position.Y < shoulderCenter.Position.Y || leftHand.Position.Y < shoulderCenter.Position.Y)
                {
                    ZoomStarted = false;
                    return false;
                }

            if (!CanTriggerZoom())
            {
                return false;
            }

            //Calculate and update hands distance
            float distance = calculateDistanceX(rightHand.Position, leftHand.Position);
            float deltaDistance = distance - HandsDistance;
            deltaDistance = normalizeDistance(deltaDistance, HandsDistanceErrorIgnored);
            HandsDistance = distance;

            //Set as zoom gesture track started
            Boolean lastZoomStarted = ZoomStarted;
            ZoomStarted = true;
            //If has not started before than just return
            if (!lastZoomStarted)
            {
                return false;
            }

            //If get here than distance was updated and changed from last frame. Trigger gesture event to listeners.
            if (deltaDistance > 0)
            {
                //TriggerGestureEvent(ZoomOut);
                LastEventName = GestureEvents.ZOOM_OUT;
            }
            else if (deltaDistance < 0)
            {
                //TriggerGestureEvent(ZoomIn);
                LastEventName = GestureEvents.ZOOM_IN;
            }
            return true;
        }

        private Boolean CanTriggerZoom()
        {
            lock (lockInterval)
            {
                long now = Time.CurrentTimeMillis();
                if (now - lastZoomTrigged > ZoomIntervalMiliseconds)
                {
                    lastZoomTrigged = now;
                    return true;
                }
                else //Dont trigger it so quickly
                {
                    return false;
                }
            }
        }

        //From diferent frames an error may occur
        private float normalizeDistance(float deltaDistance, float errorExpected)
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

        //Calculate distance of X axis
        private float calculateDistanceX(SkeletonPoint rightHandPosition, SkeletonPoint leftHandPosition)
        {
            float distance = Math.Abs(rightHandPosition.X - leftHandPosition.X);
            return distance;
        }

    }
}
