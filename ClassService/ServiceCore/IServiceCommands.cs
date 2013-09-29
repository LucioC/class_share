using System;
namespace ServiceCore
{
    public interface IServiceCommands
    {
        void ProcessCloseImage();
        void ProcessClosePresentation();

        void ProcessMoveRight(int multiplier);
        void ProcessMoveLeft(int multiplier);

        void ProcessMoveUp(int multiplier);
        void ProcessMoveDown(int multiplier);

        void ProcessRotateRight();
        void ProcessRotateLeft();

        void ProcessZoomOut(int multiplier);
        void ProcessZoomIn(int multiplier);

        void ProcessNextSlide();
        void ProcessPreviousSlide();
    }
}
