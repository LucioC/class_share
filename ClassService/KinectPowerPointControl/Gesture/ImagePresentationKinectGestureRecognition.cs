using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    public class ImagePresentationKinectGestureRecognition: AbstractKinectGestureRecognition
    {
        public ImagePresentationKinectGestureRecognition(SkeletonStateRepository skeletonRepository)
            : base(skeletonRepository)
        {
            gestureRecognizers.Add(new ZoomAndRotationGesture());
            gestureRecognizers.Add(new JoinHandsGesture());
            gestureRecognizers.Add(new CloseHandAfterTime());
            gestureRecognizers.Add(new GripHandMove());
        }
    }
}
