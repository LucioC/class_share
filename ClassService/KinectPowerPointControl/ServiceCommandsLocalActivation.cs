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
            //TODO
            //System.Windows.Forms.SendKeys.SendWait("{ESC}");
        }

        public void ProcessCloseImage()
        {
            //TODO
            this.communicator.SendMessage(ServiceCommands.CLOSE_IMAGE);
        }
    }
}
