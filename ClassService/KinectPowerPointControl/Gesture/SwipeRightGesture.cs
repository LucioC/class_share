using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using CommonUtils;

namespace KinectPowerPointControl.Gesture
{
    public class SwipeRightGesture: IGestureRecognizer
    {
        public SwipeRightGesture()
        {
            Name = GestureEvents.SWIPE_RIGHT;
        }

        int state = 0;
        SkeletonPoint initialPosition;

        public bool IdentifyGesture(Skeleton skeleton)
        {
            var rightHand = skeleton.Joints[JointType.HandRight];
            var leftHand = skeleton.Joints[JointType.HandLeft];
            var head = skeleton.Joints[JointType.Head];
            var centerShoulder = skeleton.Joints[JointType.ShoulderCenter];

            //If hand is lower than center shoulder minus 10 centimers then reset gesture
            if (rightHand.Position.Y < centerShoulder.Position.Y - 0.1)
            {
                state = 0;
                return false;
            }

            if(state == 0)
            {
                state = 1;
                initialPosition = rightHand.Position;

                Output.Debug("SwipeRight", "Identifyed hand above shoudler");

                return false;
            }

            if (state == 1)
            {
                SkeletonPoint nextPoint = rightHand.Position;

                //If hand went left then reset state
                if (nextPoint.X < initialPosition.X - 0.1)
                {
                    state = 0;
                    return false;
                }
                
                //If hand went right 30cm then swipe occured
                if (nextPoint.X > initialPosition.X + 0.3)
                {
                    //Mark as state 2, it means "already executed" so it will reset only when hand are down
                    state = 2;
                    Output.Debug("SwipeRight", "Right Gesture Executed");
                    return true;
                } 

                return false;
            }

            //Gesture if finished already, it need to be reset by hand positioning
            if (state == 2) return false;

            state = 0;
            return false;
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
