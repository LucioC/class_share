using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using CommonUtils;

namespace KinectPowerPointControl.Gesture
{
    public class ZoomGesture: IGestureRecognizer
    {
        private long lastZoomTrigged = 0;
        private System.Object lockInterval = new System.Object();

        private Boolean ZoomStarted = false;
        public float HandsDistance { get; set; }
        public float HandsDistanceErrorIgnored { get; set; }

        public ZoomGesture()
        {
            HandsDistance = 0f;
            HandsDistanceErrorIgnored = 0.05f;
            Interval = 100;
        }

        public bool IdentifyGesture(Skeleton skeleton)
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
            float distance = GestureUtils.calculateDistanceX(rightHand.Position, leftHand.Position);
            float deltaDistance = distance - HandsDistance;
            deltaDistance = GestureUtils.normalizeDistance(deltaDistance, HandsDistanceErrorIgnored);
            
            //Set as zoom gesture track started
            Boolean lastZoomStarted = ZoomStarted;
            ZoomStarted = true;

            //If has not started before than just return
            if (!lastZoomStarted)
            {
                HandsDistance = distance;
                return false;
            }

            //If get here than distance was updated and changed from last frame. Trigger gesture event to listeners.
            if (deltaDistance > 0)
            {
                //TriggerGestureEvent(ZoomOut);
                Name = GestureEvents.ZOOM_IN; HandsDistance = distance;
                return true;
            }
            else if (deltaDistance < 0)
            {
                //TriggerGestureEvent(ZoomIn);
                Name = GestureEvents.ZOOM_OUT; HandsDistance = distance;
                return true;
            }
            return false;
        }

        private Boolean CanTriggerZoom()
        {
            lock (lockInterval)
            {
                long now = Time.CurrentTimeMillis();
                if (now - lastZoomTrigged > Interval)
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
