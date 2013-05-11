using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore
{
    public class DefaultCommunicator: NonBlockingSpeaker, IListener
    {
        public void RegisterListener(IListener listener)
        {
            this.MessageSent += listener.ReceiveMessage;
        }

        public void RemoveListener(IListener listener)
        {
            this.MessageSent -= listener.ReceiveMessage;
        }

        public virtual void ReceiveMessage(string message)
        {
            
        }
    }
}
