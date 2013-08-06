using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore
{
    public class SkeletonStateRepository
    {
        //private static UserSkeletonState userState;
        public UserSkeletonState FirstUser { get; set; }

        public SkeletonStateRepository()
        {
            FirstUser = new UserSkeletonState();
        }
    }
}
