using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore.Utils;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    public class ZoomGesture: IGestureRecognizer
    {
        private long lastZoomTrigged = 0;
        private System.Object lockInterval = new System.Object();

        private int state = 0;
        private Clock time = new Clock();

        private Boolean ZoomStarted = false;
        public float HandsDistance { get; set; }
        public float HandsDistanceErrorIgnored { get; set; }

        public ZoomGesture()
        {
            HandsDistance = 0f;
            HandsDistanceErrorIgnored = 0.05f;
            Interval = 100;
        }

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            Skeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.Joints[JointType.HandRight];
            var leftHand = skeleton.Joints[JointType.HandLeft];
            var head = skeleton.Joints[JointType.Head];

            var shoulderCenter = skeleton.Joints[JointType.ShoulderCenter];

            //If hands are below shouldcenter dont track gesture 
            if (rightHand.Position.Y < shoulderCenter.Position.Y || leftHand.Position.Y < shoulderCenter.Position.Y)
            {
                ZoomStarted = false;
                state = 0;
                return false;
            }
            if (!CanTriggerZoom())
            {
                state = 0;
                return false;
            }
            if (!userState.IsLeftHandGripped || !userState.IsRightHandGripped)
            {
                state = 0;
                return false;
            }
            
            //Calculate and update hands distance
            float distance = GestureUtils.calculateDistanceX(rightHand.Position, leftHand.Position);
            float deltaDistance = distance - HandsDistance;
            deltaDistance = GestureUtils.normalizeDistance(deltaDistance, HandsDistanceErrorIgnored);

            if (state == 0)
            {
                state = 1;
                HandsDistance = distance;
                return false;
            }

            //If got here than distance was updated and changed from last frame. Trigger gesture event to listeners.
            if (deltaDistance > 0)
            {
                //TriggerGestureEvent(ZoomOut);
                Name = GestureEvents.ZOOM_IN; 
                HandsDistance = distance;
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
                long now = time.CurrentTimeInMillis();
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
