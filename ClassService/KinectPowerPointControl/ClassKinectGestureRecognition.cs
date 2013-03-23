using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using CommonUtils;

namespace KinectPowerPointControl
{
    public class ClassKinectGestureRecognition
    {
        Skeleton[] skeletons;
        bool isForwardGestureActive = false;
        bool isBackGestureActive = false;
        
        public const String ForwardGesture = "FORWARD";
        public const String BackGesture = "BACK";
        public const String ZoomIn = "ZOOMIN";
        public const String ZoomOut = "ZOOMOUT";

        private Boolean ZoomStarted = false;
        public float HandsDistance { get; set; }
        public float HandsDistanceErrorIgnored { get; set; }
        public long ZoomIntervalMiliseconds { get; set; }
        private long lastZoomTrigged = 0;

        public delegate void GestureRecognizedEvent(String gesture);
        public event GestureRecognizedEvent GestureRecognized;

        public Joint Head;
        public Joint RightHand;
        public Joint LeftHand;
        public Joint CenterShoulder;

        private Output output;

        private System.Object lockInterval = new System.Object();

        public ClassKinectGestureRecognition()
        {
            output = new Output();
            HandsDistance = 0f;
            HandsDistanceErrorIgnored = 0.05f;
            ZoomIntervalMiliseconds = 200;
        }

        public void ProcessFrameReady(SkeletonFrame skeletonFrame)
        {
            if (skeletons == null ||
                    skeletons.Length != skeletonFrame.SkeletonArrayLength)
            {
                skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
            }

            skeletonFrame.CopySkeletonDataTo(skeletons);

            Skeleton closestSkeleton = (from s in skeletons
                                        where s.TrackingState == SkeletonTrackingState.Tracked &&
                                              s.Joints[JointType.Head].TrackingState == JointTrackingState.Tracked
                                        select s).OrderBy(s => s.Joints[JointType.Head].Position.Z)
                                                .FirstOrDefault();

            if (closestSkeleton == null)
                return;

            var head = closestSkeleton.Joints[JointType.Head];
            var rightHand = closestSkeleton.Joints[JointType.HandRight];
            var leftHand = closestSkeleton.Joints[JointType.HandLeft];
            var shoulderCenter = closestSkeleton.Joints[JointType.ShoulderCenter];

            if (head.TrackingState != JointTrackingState.Tracked ||
                rightHand.TrackingState != JointTrackingState.Tracked ||
                leftHand.TrackingState != JointTrackingState.Tracked || 
                shoulderCenter.TrackingState != JointTrackingState.Tracked)
            {
                //Don't have a good read on the joints so we cannot process gestures
                return;
            }
            
            Head = head;
            RightHand = rightHand;
            LeftHand = leftHand;
            CenterShoulder = shoulderCenter;

            //Process gestures, if one is detected dont do another
            if (ProcessForwardBackGesture(head, rightHand, leftHand))
            {
                output.WriteToDebugOrConsole("Forward or Back Gesture was identified");
            }
            else
            {
                ProcessZoomGestures(closestSkeleton);
            }
        }

        private void TriggerGestureEvent(String gesture)
        {
            //Trigger event so the recognized gesture can be processed and generate some action outside of this class
            if (this.GestureRecognized != null)
            {
                this.GestureRecognized(gesture);
            }
        }

        private Boolean ProcessZoomGestures(Skeleton skeleton)
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
            if(rightHand.Position.Y < shoulderCenter.Position.Y || leftHand.Position.Y < shoulderCenter.Position.Y)
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
                TriggerGestureEvent(ZoomOut);
            }
            else if (deltaDistance < 0)
            {
                TriggerGestureEvent(ZoomIn);
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

        //From diferents frames an error may occur
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

        //Process a forward or back gesture
        private Boolean ProcessForwardBackGesture(Joint head, Joint rightHand, Joint leftHand)
        {
            if (rightHand.Position.X > head.Position.X + 0.45)
            {
                if (!isBackGestureActive && !isForwardGestureActive)
                {
                    isForwardGestureActive = true;
                    TriggerGestureEvent(ForwardGesture);
                    return true;
                }
            }
            else
            {
                isForwardGestureActive = false;
            }

            if (leftHand.Position.X < head.Position.X - 0.45)
            {
                if (!isBackGestureActive && !isForwardGestureActive)
                {
                    isBackGestureActive = true;
                    TriggerGestureEvent(BackGesture);
                    return true;
                }
            }
            else
            {
                isBackGestureActive = false;
            }
            return false;
        }
    }
}
