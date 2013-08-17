using System;
namespace KinectPowerPointControl.Gesture
{
    public interface IRotationGripGesture: IGestureRecognizer
    {
        float CurrentAngleDelta { get; set; }
        bool IdentifyRotationGesture(ServiceCore.IJoint rightHand, ServiceCore.IJoint leftHand, ServiceCore.IJoint spine, bool isLeftHandGripped, bool isRightHandGripped);
    }
}
