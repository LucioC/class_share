using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore.Utils;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    public class OutstretchedArmMovementGesture: IGestureRecognizer
    {
        public OutstretchedArmMovementGesture()
        {
            Name = GestureEvents.OUTSTRETCHED_ARM;
            State = 0;
        }

        public int State { get; set; }
        private SkeletonPoint initialPoint;
        
        private float percentage = 0.7f;

        private float distanceIntervalToMovement = 0.08f;

        public bool IdentifyGesture(UserSkeletonState userState)
        {
            ISkeleton skeleton = userState.Skeleton;
            var rightHand = skeleton.HandRight;
            var leftHand = skeleton.HandLeft;
            var rightElbow = skeleton.ElbowRight;
            var leftElbow = skeleton.ElbowLeft;
            var rightShoulder = skeleton.ShoulderRight;
            var leftShoulder = skeleton.ShoulderLeft;
            var spine = skeleton.Spine;

            Double distanceRightShoulderAndElbow = KinectUtils.DistanceBetweenSkeletonPoints(rightShoulder.Position,rightElbow.Position);
            distanceRightShoulderAndElbow = Math.Abs(distanceRightShoulderAndElbow);

            Double distanceRightElbowAndHand = KinectUtils.DistanceBetweenSkeletonPoints(rightElbow.Position, rightHand.Position);
            distanceRightElbowAndHand = Math.Abs(distanceRightElbowAndHand);

            Double distanceZRightShoulderAndElbow = rightShoulder.Position.Z - rightElbow.Position.Z;
            distanceZRightShoulderAndElbow = Math.Abs(distanceZRightShoulderAndElbow);

            Double distanceZRightElbowAndHand = rightElbow.Position.Z - rightHand.Position.Z;
            distanceZRightElbowAndHand = Math.Abs(distanceZRightElbowAndHand);

            if (distanceZRightShoulderAndElbow > distanceRightShoulderAndElbow * percentage
                && distanceZRightElbowAndHand > distanceRightElbowAndHand * percentage)
            {
                if (State != 1)
                {
                    State = 1;
                    Output.Debug("OutstretchedArmMovement", "Is Stresched");
                    initialPoint = rightHand.Position;
                    return true;
                }
            }
            else
            {
                State = 0;
                return false;
            }

            if (State == 1)
            {
                double deltaX = rightHand.Position.X - initialPoint.X;
                double deltaY = rightHand.Position.Y - initialPoint.Y;

                if (deltaX > distanceIntervalToMovement)
                {
                    Name = GestureEvents.MOVE_LEFT;
                    initialPoint.X += distanceIntervalToMovement;
                    return true;
                }
                else if (deltaX < -distanceIntervalToMovement)
                {
                    Name = GestureEvents.MOVE_RIGHT;
                    initialPoint.X -= distanceIntervalToMovement;
                    return true;
                }
                else if (deltaY > distanceIntervalToMovement)
                {
                    Name = GestureEvents.MOVE_DOWN;
                    initialPoint.Y += distanceIntervalToMovement;
                    return true;
                }
                else if (deltaY < -distanceIntervalToMovement)
                {
                    Name = GestureEvents.MOVE_UP;
                    initialPoint.Y -= distanceIntervalToMovement;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
            if (State != 0)
                Output.Debug("OutstretchedArmMovement", "Arm not stretched");

            State = 0;
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
