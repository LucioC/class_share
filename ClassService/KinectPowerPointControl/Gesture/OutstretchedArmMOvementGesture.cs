using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using ServiceCore.Utils;

namespace KinectPowerPointControl.Gesture
{
    public class OutstretchedArmMovementGesture: IGestureRecognizer
    {
        public OutstretchedArmMovementGesture()
        {
            Name = GestureEvents.OUTSTRETCHED_ARM;
        }

        private int state = 0;
        private SkeletonPoint initialPoint;
        
        private float percentage = 0.7f;

        private float distanceIntervalToMovement = 0.08f;

        public bool IdentifyGesture(Skeleton skeleton)
        {
            var rightHand = skeleton.Joints[JointType.HandRight];
            var leftHand = skeleton.Joints[JointType.HandLeft];
            var rightElbow = skeleton.Joints[JointType.ElbowRight];
            var leftElbow = skeleton.Joints[JointType.ElbowLeft];
            var rightShoulder = skeleton.Joints[JointType.ShoulderRight];
            var leftShoulder = skeleton.Joints[JointType.ShoulderLeft];
            var spine = skeleton.Joints[JointType.Spine];

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
                if (state != 1)
                {
                    state = 1;
                    Output.Debug("OutstretchedArmMovement", "Is Stresched");
                    initialPoint = rightHand.Position;
                    return true;
                }
            }
            else
            {
                state = 0;
                return false;
            }

            if (state == 1)
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
            
            if (state != 0)
                Output.Debug("OutstretchedArmMovement", "Arm not stretched");

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
