using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace ServiceCore
{
    public class KinectJointWrapper: IJoint
    {
        Joint kinectJoint;

        public KinectJointWrapper(Joint joint)
        {
            this.kinectJoint = joint;
        }

        public SkeletonPoint Position
        {
            get { return kinectJoint.Position; }
        }
    }
}
