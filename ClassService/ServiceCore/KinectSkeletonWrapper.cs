using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace ServiceCore
{
    public class KinectSkeletonWrapper: ISkeleton
    {
        Skeleton kinectSkeleton;
        public KinectSkeletonWrapper(Skeleton skeleton)
        {
            kinectSkeleton = skeleton;
        }

        public Joint HandRight
        {
            get { return kinectSkeleton.Joints[JointType.HandRight]; }
        }

        public Joint HandLeft
        {
            get { return kinectSkeleton.Joints[JointType.HandLeft]; }
        }

        public Joint AnkleLeft
        {
            get { return kinectSkeleton.Joints[JointType.AnkleLeft]; }
        }

        public Joint AnkleRight
        {
            get { return kinectSkeleton.Joints[JointType.AnkleRight]; }
        }

        public Joint ElbowRight
        {
            get { return kinectSkeleton.Joints[JointType.ElbowRight]; }
        }

        public Joint ElbowLeft
        {
            get { return kinectSkeleton.Joints[JointType.ElbowLeft]; }
        }

        public Joint Spine
        {
            get { return kinectSkeleton.Joints[JointType.Spine]; }
        }

        public Joint Head
        {
            get { return kinectSkeleton.Joints[JointType.Head]; }
        }

        public Joint FootLeft
        {
            get { return kinectSkeleton.Joints[JointType.FootLeft]; }
        }

        public Joint FootRight
        {
            get { return kinectSkeleton.Joints[JointType.FootRight]; }
        }

        public Joint HipCenter
        {
            get { return kinectSkeleton.Joints[JointType.HipCenter]; }
        }

        public Joint HipRight
        {
            get { return kinectSkeleton.Joints[JointType.HipRight]; }
        }

        public Joint HipLeft
        {
            get { return kinectSkeleton.Joints[JointType.HipLeft]; }
        }

        public Joint KneeRight
        {
            get { return kinectSkeleton.Joints[JointType.KneeRight]; }
        }

        public Joint KneeLeft
        {
            get { return kinectSkeleton.Joints[JointType.KneeLeft]; }
        }

        public Joint ShoulderCenter
        {
            get { return kinectSkeleton.Joints[JointType.ShoulderCenter]; }
        }

        public Joint ShoulderRight
        {
            get { return kinectSkeleton.Joints[JointType.ShoulderRight]; }
        }

        public Joint ShoulderLeft
        {
            get { return kinectSkeleton.Joints[JointType.ShoulderLeft]; }
        }

        public Joint WristLeft
        {
            get { return kinectSkeleton.Joints[JointType.WristLeft]; }
        }

        public Joint WristRight
        {
            get { return kinectSkeleton.Joints[JointType.WristRight]; }
        }
    }
}
