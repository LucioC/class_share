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
        Object skeletonLock = new Object();
        private ISkeleton skeleton;
        public ISkeleton Skeleton
        {
            get 
            {
                lock (skeletonLock)
                {
                    return skeleton;
                }
            }
            set
            {
                lock (skeletonLock)
                {
                    skeleton = value;

                }
            }
        }

        Object userLock = new Object();
        private UserInfo userState;
        /// <summary>
        /// User State Info.
        /// Should check if is null.
        /// </summary>
        public UserInfo UserState { 
            get
            {
                lock (userLock)
                {
                    return userState;
                }
            } 
            set
            {
                lock (userLock)
                {
                    userState = value;
                }
            }
        }

        Object handRight = new Object();
        private Boolean isRightHandGripped;
        public Boolean IsRightHandGripped
        {
            get
            {
                lock (handRight)
                {
                    return isRightHandGripped;
                }
            }
            set
            {
                lock (handRight)
                {
                    isRightHandGripped = value;
                }
            }
        }

        Object handLeft = new Object();
        private Boolean isLeftHandGripped;
        public Boolean IsLeftHandGripped
        {
            get
            {
                lock (handLeft)
                {
                    return isLeftHandGripped;
                }
            }
            set
            {
                lock (handLeft)
                {
                    isLeftHandGripped = value;
                }
            }
        }

        public UserSkeletonState()
        {
            skeleton = new KinectSkeletonWrapper(new Skeleton());
            userState = null;
            isLeftHandGripped = false;
            isRightHandGripped = false;
        }
    }
}
