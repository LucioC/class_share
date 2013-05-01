using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore
{
    public class AbstractCommunicator: AbstractNonBlockingSpeaker, IListener
    {
        public virtual void ReceiveMessage(string message)
        {
            
        }
    }
}
