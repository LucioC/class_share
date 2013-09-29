using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceCore;

namespace KinectPowerPointControl
{
    public class ServiceCommandsLocalActivation: IServiceCommands
    {
        private CommandExecutor executor;

        public ServiceCommandsLocalActivation(CommandExecutor executor)
        {
            this.executor += executor;
        }

        public void setListeners(CommandExecutor executors)
        {
            this.executor += executor;
        }

        private void SendCommandAssynchrnously(String command, String value)
        {
            this.executor.BeginInvoke(command, value, null, null);
        }

        public void ProcessClosePresentation()
        {
            SendCommandAssynchrnously(ServiceCommands.CLOSE_PRESENTATION, "");
        }

        public void ProcessCloseImage()
        {
            SendCommandAssynchrnously(ServiceCommands.CLOSE_IMAGE, "");
        }
        
        public void ProcessNextSlide()
        {
            SendCommandAssynchrnously(ServiceCommands.NEXT_SLIDE, "");
        }

        public void ProcessPreviousSlide()
        {
            SendCommandAssynchrnously(ServiceCommands.PREVIOUS_SLIDE, "");
        }
        
        public void ProcessMoveRight()
        {
            SendCommandAssynchrnously(ServiceCommands.IMAGE_MOVE_X, "0.1");
        }

        public void ProcessMoveLeft()
        {
            SendCommandAssynchrnously(ServiceCommands.IMAGE_MOVE_X, "-0.1");
        }

        public void ProcessMoveUp()
        {
            SendCommandAssynchrnously(ServiceCommands.IMAGE_MOVE_Y, "-0.1");
        }

        public void ProcessMoveDown()
        {
            SendCommandAssynchrnously(ServiceCommands.IMAGE_MOVE_Y, "0.1");
        }

        public void ProcessRotateRight()
        {
            SendCommandAssynchrnously(ServiceCommands.IMAGE_ROTATE, "90");
        }

        public void ProcessRotateLeft()
        {
            SendCommandAssynchrnously(ServiceCommands.IMAGE_ROTATE, "-90");
        }

        public void ProcessZoomOut()
        {
            SendCommandAssynchrnously(ServiceCommands.IMAGE_ZOOM, "-0.1");
        }

        public void ProcessZoomIn()
        {
            SendCommandAssynchrnously(ServiceCommands.IMAGE_ZOOM, "0.1");
        }
    }
}
