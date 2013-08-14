using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect.Toolkit.Interaction;

namespace KinectPowerPointControl.Gesture
{
    public class HandsUtil
    {
        public static InteractionHandPointer GetRightHand(UserInfo userInfo)
        {
            InteractionHandPointer rightHand = null;
            try
            {
                rightHand = userInfo.HandPointers.Single(t => t.HandType == InteractionHandType.Right);
            }
            catch(Exception e)
            {
                return null;
            }
            return rightHand;
        }

        public static InteractionHandPointer GetLeftHand(UserInfo userInfo)
        {
            InteractionHandPointer leftHand = null;
            try
            {
                leftHand = userInfo.HandPointers.Single(t => t.HandType == InteractionHandType.Left);
            }
            catch (Exception e)
            {
                return null;
            }
            return leftHand;
        }

        public static Boolean IsInGrip(InteractionHandPointer hand)
        {
            return (hand.HandEventType == InteractionHandEventType.Grip);
        }
    }
}
