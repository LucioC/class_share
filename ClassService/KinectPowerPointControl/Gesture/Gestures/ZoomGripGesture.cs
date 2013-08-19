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

        public ZoomGripGesture()
        {
            HandsDistance = 0f;
            HandsDistanceErrorIgnored = 0.05f;
            Interval = 100;
            State = 0;
        }

        public bool IdentiftZoomGesture(IJoint rightHand, IJoint leftHand, IJoint spine, Boolean isLeftHandGripped, Boolean isRightHandGripped)
        {
            //If hands are below spine dont track gesture 
            if (rightHand.Position.Y < spine.Position.Y || leftHand.Position.Y < spine.Position.Y)
            {
                State = 0;
                return false;
            }
            if (!isLeftHandGripped || !isRightHandGripped)
            {
                State = 0;
                return false;
            }

            //Calculate and update hands distance
            float distance = GestureUtils.calculateDistanceX(rightHand.Position, leftHand.Position);
            float deltaDistance = distance - HandsDistance;
            deltaDistance = GestureUtils.normalizeDistance(deltaDistance, HandsDistanceErrorIgnored);

            if (State == 0)
            {
                State = 1;
                HandsDistance = distance;
                return false;
            }

            //If got here than distance was updated and changed from last frame. Trigger gesture event to listeners.
            if (deltaDistance > 0)
            {
                Name = GestureEvents.ZOOM_IN;
                HandsDistance = distance;
                return true;
            }
            else if (deltaDistance < 0)
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

            return IdentiftZoomGesture(rightHand, leftHand, spine, userState.IsLeftHandGripped, userState.IsRightHandGripped);
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
