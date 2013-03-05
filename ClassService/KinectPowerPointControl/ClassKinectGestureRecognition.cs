using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace KinectPowerPointControl
{
    public class ClassKinectGestureRecognition
    {
        Skeleton[] skeletons;
        bool isForwardGestureActive = false;
        bool isBackGestureActive = false;
        
        public const String ForwardGesture = "FORWARD";
        public const String BackGesture = "BACK";

        public delegate void GestureRecognizedEvent(String gesture);
        public event GestureRecognizedEvent GestureRecognized;

        public delegate void JointsReadyEvent();
        public event JointsReadyEvent JointsUpdatedEvent;

        public Joint Head;
        public Joint RightHand;
        public Joint LeftHand;       

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

            if (head.TrackingState != JointTrackingState.Tracked ||
                rightHand.TrackingState != JointTrackingState.Tracked ||
                leftHand.TrackingState != JointTrackingState.Tracked)
            {
                //Don't have a good read on the joints so we cannot process gestures
                return;
            }

            Head = head;
            RightHand = rightHand;
            LeftHand = leftHand;

            if (this.JointsUpdatedEvent != null)
            {
                this.JointsUpdatedEvent();
            }

            //Update Right hand, left hand, and head positions for tracking and image 
            //SetEllipsePosition(ellipseHead, head, false);
            //SetEllipsePosition(ellipseLeftHand, leftHand, isBackGestureActive);
            //SetEllipsePosition(ellipseRightHand, rightHand, isForwardGestureActive);

            //Process gesture for each skeleton track event
            ProcessForwardBackGesture(head, rightHand, leftHand);
        }

        private void TriggerGestureEvent(String gesture)
        {
            //Trigger event so the recognized gesture can be processed and generate some action outside of this class
            if (this.GestureRecognized != null)
            {
                this.GestureRecognized(gesture);
            }
        }

        //Process a forward or back gesture
        private void ProcessForwardBackGesture(Joint head, Joint rightHand, Joint leftHand)
        {
            if (rightHand.Position.X > head.Position.X + 0.45)
            {
                if (!isBackGestureActive && !isForwardGestureActive)
                {
                    isForwardGestureActive = true;
                    TriggerGestureEvent(ForwardGesture);
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
                }
            }
            else
            {
                isBackGestureActive = false;
            }
        }
    }
}
