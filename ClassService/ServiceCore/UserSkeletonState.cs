using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Interaction;

namespace ServiceCore
{
    public class UserSkeletonState
    {
        public Skeleton Skeleton { get; set; }
        public UserInfo UserState { get; set; }        
    }
}
