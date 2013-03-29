using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using CommonUtils;
using System.Collections;
using System.Collections.Specialized;

namespace KinectPowerPointControl.Gesture
{
    public class ClassKinectGestureRecognition
    {
        Skeleton[] skeletons;
        List<IGestureRecognizer> gestureRecognizers;

        public delegate void GestureRecognizedEvent(String gesture);
        public event GestureRecognizedEvent GestureRecognized;

        public Joint Head { get; set; }
        public Joint RightHand { get; set; }
        public Joint LeftHand { get; set; }
        public Joint CenterShoulder { get; set; }

        public ClassKinectGestureRecognition()
        {
            gestureRecognizers = new List<IGestureRecognizer>();
            gestureRecognizers.Add(new SwipeRightGesture());
            gestureRecognizers.Add(new SwipeLeftGesture());
        }

        public void ProcessFrameReady(SkeletonFrame skeletonFrame)
        {
            if (skeletons == null ||
                    skeletons.Length != skeletonFrame.SkeletonArrayLength)
            {
                skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
            }

            skeletonFrame.CopySkeletonDataTo(skeletons);

            //Get closest Skeleton needed
            Skeleton closestSkeleton = (from s in skeletons
                                        where s.TrackingState == SkeletonTrackingState.Tracked &&
                                              s.Joints[JointType.Head].TrackingState == JointTrackingState.Tracked
                                        select s).OrderBy(s => s.Joints[JointType.Head].Position.Z)
                                                .FirstOrDefault();

            if (closestSkeleton == null)
                return;

            //Verify if some skeleton joints are correctly tracked
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

            foreach (IGestureRecognizer recognizer in gestureRecognizers)
            {
                if(recognizer.IdentifyGesture(closestSkeleton))
                {
                    //Assume the recognizer has a name globally identifiable
                    TriggerGestureEvent(recognizer.Name);
                    break;
                }
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

    }
}
