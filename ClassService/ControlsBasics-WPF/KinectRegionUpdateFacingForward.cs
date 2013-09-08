using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect.Toolkit.Controls;
using Microsoft.Kinect;
using ServiceCore;
using KinectPowerPointControl.Gesture;

namespace Microsoft.Samples.Kinect.ControlsBasics
{
    public class KinectRegionUpdateFacingForward: KinectRegion
    {
        public SkeletonStateRepository Repository { get; set; }

        override protected void CheckIfUserIsFacingForward(Skeleton[] skeletons)
        {
            //Get closest Skeleton needed
            Skeleton closestSkeleton = (from s in skeletons
                                        where s.TrackingState == SkeletonTrackingState.Tracked &&
                                              s.Joints[JointType.Head].TrackingState == JointTrackingState.Tracked
                                        select s).OrderBy(s => s.Joints[JointType.Head].Position.Z)
                                                .FirstOrDefault();

            if (closestSkeleton == null)
                return;

            if (Repository != null)
            {
                Repository.FirstUser.IsFacingForward = GestureUtils.IsUserFacingForward(closestSkeleton);
            }
        }
    }
}
