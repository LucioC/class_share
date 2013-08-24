using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceCore;

namespace KinectPowerPointControl
{
    public class ServiceCommandsLocalActivation: ServiceCommandToKeyEvent, IServiceCommands
    {
        private DefaultCommunicator communicator;

        public ServiceCommandsLocalActivation(MessageEvent messagesListeners)
        {
            communicator = new DefaultCommunicator();
            communicator.MessageSent += messagesListeners;
        }

        public void setListeners(MessageEvent messagesListeners)
        {
            communicator.MessageSent += messagesListeners;
        }

        public void ProcessClosePresentation()
        {
            this.communicator.SendMessage(ServiceCommands.CLOSE_PRESENTATION);
        }

        public void ProcessCloseImage()
        {
            this.communicator.SendMessage(ServiceCommands.CLOSE_IMAGE);
        }
        
        public void ProcessNextSlide()
        {
            this.communicator.SendMessage(ServiceCommands.NEXT_SLIDE);
        }

        public void ProcessPreviousSlide()
        {
            this.communicator.SendMessage(ServiceCommands.PREVIOUS_SLIDE);
        }
    }
}
