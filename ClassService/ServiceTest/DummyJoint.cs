using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceCore;
using Microsoft.Kinect;

namespace ServiceTest
{
    public class DummyJoint: IJoint
    {
        SkeletonPoint point;
        public DummyJoint(SkeletonPoint point)
        {
            this.point = point;
        }

        public SkeletonPoint Position
        {
            get { return this.point; }
        }
    }
}
