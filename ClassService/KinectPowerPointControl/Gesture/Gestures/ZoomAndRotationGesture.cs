using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceCore;

namespace KinectPowerPointControl.Gesture
{
    public class ZoomAndRotationGesture : IGestureRecognizer
    {
        private IGestureRecognizer zoomIdentifier;
        private IRotationGripGesture rotationIdentifier;

        public float AngleControl { get; set; }

        private bool hasZoomStarted = false;
        private bool hasRotationStarted = false;

        public float CurrentDeltaAngle { get { return rotationIdentifier.CurrentAngleDelta; } }

        public ZoomAndRotationGesture()
        {
            zoomIdentifier = new ZoomGripGesture();
            rotationIdentifier = new RotationGripGesture();
            AngleControl = 10;
        }

        public ZoomAndRotationGesture(IGestureRecognizer zoomIdentifier, IRotationGripGesture rotationIdentifier)
        {
            this.zoomIdentifier = zoomIdentifier;
            this.rotationIdentifier = rotationIdentifier;
            AngleControl = 10;
        }

        public string Name
        {
            get;
            set;
        }

        public long Interval
        {
            get;
            set;
        }

        private bool rotated = false;
        private bool zoomed = false;

        private bool isZomming = false;

        public bool IdentifyGesture(ServiceCore.UserSkeletonState userSkeletonState)
        {
            rotated = this.rotationIdentifier.IdentifyGesture(userSkeletonState);
            if (Math.Abs(rotationIdentifier.CurrentAngleDelta) > AngleControl && !isZomming)
            {
                if (rotated)
                {
                    Name = this.rotationIdentifier.Name;
                    return true;
                }

                return false;
            }

            zoomed = this.zoomIdentifier.IdentifyGesture(userSkeletonState);
            isZomming = (this.zoomIdentifier.State > 0) ? true : false;
            if (zoomed)
            {
                isZomming = true;
                Name = this.zoomIdentifier.Name;
                return true;
            }

            return false;
        }

        public int State
        {
            get;
            set;
        }
    }
}
