using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectPowerPointControl.Gesture
{
    public class ImagePresentationKinectGestureRecognition: AbstractKinectGestureRecognition
    {
        public ImagePresentationKinectGestureRecognition(): base()
        {
            gestureRecognizers.Add(new ZoomGesture());
            gestureRecognizers.Add(new RotationGesture());
            gestureRecognizers.Add(new Move());
            gestureRecognizers.Add(new JoinHandsGesture());
        }
    }
}
