using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore.Utils;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    public class ZoomGripGesture: IGestureRecognizer
    {
        private System.Object lockInterval = new System.Object();

        public int State { get; set; }
        private Clock time = new Clock();

        public float HandsDistance { get; set; }
        public float HandsDistanceErrorIgnored { get; set; }
        public float MaximumHandsYDistanceToStart { get; set; }

        public ZoomGripGesture()
        {
            HandsDistance = 0f;
            HandsDistanceErrorIgnored = 0.05f;
            Interval = 100;
            MaximumHandsYDistanceToStart = 0.12f;
            State = 0;
        }

        public bool IdentifyZoomGesture(IJoint rightHand, IJoint leftHand, IJoint spine, Boolean isLeftHandGripped, Boolean isRightHandGripped)
        {
            //If hands are below spine dont track gesture 
            if (GestureUtils.IsOneHandBelowSpine(rightHand, leftHand, spine))
            {
                State = 0;
                return false;
            }
            if (!isLeftHandGripped || !isRightHandGripped)
            {
                State = 0;
                return false;
            }

            var handInFront = (rightHand.Position.Z > leftHand.Position.Z) ? rightHand : leftHand;
            float halfDistanceBetweenHandAndSpine = GestureUtils.HalfDistanceBetweenHandAndSpine(handInFront, spine);
            if (GestureUtils.AreHandsSeparatedInZ(rightHand, leftHand, halfDistanceBetweenHandAndSpine) )
            {
                State = 0;
                return false;
            }

            //If gesture is not being executed yet, then if hands are separated in Y, dont start it
            if (State == 0)
            {
                if (GestureUtils.AreHandsSeparatedInY(rightHand, leftHand, MaximumHandsYDistanceToStart))
                {
                    return false;
                }
            }

            //Calculate and update hands distance
            float distance = GestureUtils.DistanceXYBetweenPoints(rightHand.Position, leftHand.Position);
            float deltaXDistance = distance - HandsDistance;
            deltaXDistance = GestureUtils.NormalizeDistance(deltaXDistance, HandsDistanceErrorIgnored);

            if (State == 0)
            {
                State = 1;
                HandsDistance = distance;
                return false;
            }

            //If got here than distance was updated and changed from last frame. Trigger gesture event to listeners.
            if (deltaXDistance > 0)
            {
                Name = GestureEvents.ZOOM_IN;
                HandsDistance = distance;
                return true;
            }
            else if (deltaXDistance < 0)
            {
                Name = GestureEvents.ZOOM_OUT; 
                HandsDistance = distance;
                return true;
            }
            return false;
        }

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            ISkeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.HandRight;
            var leftHand = skeleton.HandLeft;
            var head = skeleton.Head;
            var spine = skeleton.Spine;

            return IdentifyZoomGesture(rightHand, leftHand, spine, userState.IsLeftHandGripped, userState.IsRightHandGripped);
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
