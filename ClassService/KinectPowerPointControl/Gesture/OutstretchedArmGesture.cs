using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using CommonUtils;

namespace KinectPowerPointControl.Gesture
{
    public class OutstretchedArmGesture: IGestureRecognizer
    {
        public OutstretchedArmGesture()
        {
            Name = GestureEvents.OUTSTRETCHED_ARM;
        }

        int state = 0;

        float percentage = 0.7f;

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
                //Already fired that
                if (state == 1)
                {
                    return false;
                }
                {
                    state = 1;
                    Output.Debug("OutstretchedArm", "Is Stresched");
                    return true;
                }
            }

            if (state != 0)
                Output.Debug("OutstretchedArm", "Arm not stretched");

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
