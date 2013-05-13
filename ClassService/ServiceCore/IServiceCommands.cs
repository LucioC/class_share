using System;
namespace ServiceCore
{
    public interface IServiceCommands
    {
        void ProcessCloseImage();
        void ProcessClosePresentation();

        void ProcessMoveRight();
        void ProcessMoveLeft();

        void ProcessMoveUp();
        void ProcessMoveDown();

        void ProcessRotateRight();
        void ProcessRotateLeft();

        void ProcessZoomOut();
        void ProcessZoomIn();

        void ProcessNextSlide();
        void ProcessPreviousSlide();
    }
}
