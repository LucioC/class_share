using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceCore;
using ServiceCore.Utils;

namespace KinectPowerPointControl.Gesture
{
    public class ZoomAndRotationGesture : IGestureRecognizer
    {
        private IGestureRecognizer zoomIdentifier;
        private IRotationGripGesture rotationIdentifier;

        public float AngleControl { get; set; }

        public float CurrentDeltaAngle { get { return rotationIdentifier.CurrentAngleDelta; } }

        public ZoomAndRotationGesture()
        {
            zoomIdentifier = new ZoomGripGesture();
            rotationIdentifier = new RotationGripGesture();
            AngleControl = 5;
        }

        public ZoomAndRotationGesture(IGestureRecognizer zoomIdentifier, IRotationGripGesture rotationIdentifier)
        {
            this.zoomIdentifier = zoomIdentifier;
            this.rotationIdentifier = rotationIdentifier;
            AngleControl = 5;
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

        private bool hadZoomed = false;
        private bool hadRotated = false;

        public bool IdentifyGesture(ServiceCore.UserSkeletonState userSkeletonState)
        {
            rotated = this.rotationIdentifier.IdentifyGesture(userSkeletonState);
            hadRotated = (this.rotationIdentifier.State == 0) ? false : hadRotated;
            if (Math.Abs(rotationIdentifier.CurrentAngleDelta) > AngleControl && !hadZoomed)
            {
                //Output.Debug("ZoomAndRotation","Can rotate. angle:" + rotationIdentifier.CurrentAngleDelta);
                if (rotated)
                {
                    Output.Debug("ZoomAndRotation", "ROTATED");
                    hadRotated = true;
                    Name = this.rotationIdentifier.Name;
                    return true;
                }

                return false;
            }

            if (!hadRotated)
            {
                zoomed = this.zoomIdentifier.IdentifyGesture(userSkeletonState);
                hadZoomed = (this.zoomIdentifier.State == 0) ? false : hadZoomed;
                if (zoomed)
                {
                    Output.Debug("ZoomAndRotation", "Zommed");
                    Output.Debug("ZoomAndRotation", "Rotation angle was :" + rotationIdentifier.CurrentAngleDelta + " || rotation state : " + rotationIdentifier.State);
                    hadZoomed = true;
                    Name = this.zoomIdentifier.Name;
                    return true;
                }
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
