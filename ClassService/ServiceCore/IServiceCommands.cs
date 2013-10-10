using System;
namespace ServiceCore
{
    public interface IServiceCommands
    {
        void ProcessCloseImage();
        void ProcessClosePresentation();

        void ProcessMoveRight(float multiplier);
        void ProcessMoveLeft(float multiplier);

        void ProcessMoveUp(float multiplier);
        void ProcessMoveDown(float multiplier);

        void ProcessRotateRight();
        void ProcessRotateLeft();

        void ProcessZoomOut(float multiplier);
        void ProcessZoomIn(float multiplier);

        void ProcessNextSlide();
        void ProcessPreviousSlide();
    }
}
