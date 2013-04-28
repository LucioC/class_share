using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect.Toolkit.Interaction;

namespace KinectPowerPointControl
{
    public class InteractionClient: IInteractionClient
    {
        public InteractionInfo GetInteractionInfoAtLocation(int skeletonTrackingId, InteractionHandType handType, double x, double y)
        {
            var interactionInfo = new InteractionInfo
            {
                IsPressTarget = true,
                IsGripTarget = true
            };

            return interactionInfo;
        }
    }
}
