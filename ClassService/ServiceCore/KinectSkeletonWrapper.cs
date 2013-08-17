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

        public IJoint HandRight
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.HandRight]); }
        }

        public IJoint HandLeft
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.HandLeft]); }
        }

        public IJoint AnkleLeft
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.AnkleLeft]); }
        }

        public IJoint AnkleRight
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.AnkleRight]); }
        }

        public IJoint ElbowRight
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.ElbowRight]); }
        }

        public IJoint ElbowLeft
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.ElbowLeft]); }
        }

        public IJoint Spine
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.Spine]); }
        }

        public IJoint Head
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.Head]); }
        }

        public IJoint FootLeft
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.FootLeft]); }
        }

        public IJoint FootRight
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.FootRight]); }
        }

        public IJoint HipCenter
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.HipCenter]); }
        }

        public IJoint HipRight
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.HipRight]); }
        }

        public IJoint HipLeft
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.HipLeft]); }
        }

        public IJoint KneeRight
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.KneeRight]); }
        }

        public IJoint KneeLeft
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.KneeLeft]); }
        }

        public IJoint ShoulderCenter
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.ShoulderCenter]); }
        }

        public IJoint ShoulderRight
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.ShoulderRight]); }
        }

        public IJoint ShoulderLeft
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.ShoulderLeft]); }
        }

        public IJoint WristLeft
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.WristLeft]); }
        }

        public IJoint WristRight
        {
            get { return new KinectJointWrapper(kinectSkeleton.Joints[JointType.WristRight]); }
        }
    }
}
