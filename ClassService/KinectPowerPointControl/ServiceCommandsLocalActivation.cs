﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceCore;
using System.Globalization;
using ServiceCore.Utils;

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
            Output.Debug("ServiceLocalCommand","Command: " + command + " with param " + value);
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
        
        public void ProcessMoveRight(float multiplier=1)
        {
            float value = 0.1f*multiplier;
            SendCommandAssynchrnously(ServiceCommands.IMAGE_MOVE_X, value.ToString(CultureInfo.InvariantCulture));
        }

        public void ProcessMoveLeft(float multiplier = 1)
        {
            float value = -0.1f * multiplier;
            SendCommandAssynchrnously(ServiceCommands.IMAGE_MOVE_X, value.ToString(CultureInfo.InvariantCulture));
        }

        public void ProcessMoveUp(float multiplier = 1)
        {
            float value = -0.1f * multiplier;
            SendCommandAssynchrnously(ServiceCommands.IMAGE_MOVE_Y, value.ToString(CultureInfo.InvariantCulture));
        }

        public void ProcessMoveDown(float multiplier = 1)
        {
            float value = 0.1f * multiplier;
            SendCommandAssynchrnously(ServiceCommands.IMAGE_MOVE_Y, value.ToString(CultureInfo.InvariantCulture));
        }

        public void ProcessRotateRight()
        {
            SendCommandAssynchrnously(ServiceCommands.IMAGE_ROTATE, "90");
        }

        public void ProcessRotateLeft()
        {
            SendCommandAssynchrnously(ServiceCommands.IMAGE_ROTATE, "-90");
        }

        public void ProcessZoomOut(float multiplier = 0.1f)
        {
            multiplier = -multiplier;
            SendCommandAssynchrnously(ServiceCommands.IMAGE_ZOOM, multiplier.ToString(CultureInfo.InvariantCulture));
        }

        public void ProcessZoomIn(float multiplier = 0.1f)
        {
            SendCommandAssynchrnously(ServiceCommands.IMAGE_ZOOM, multiplier.ToString(CultureInfo.InvariantCulture));
        }
    }
}
