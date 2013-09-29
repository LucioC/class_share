using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore
{
    public class NonBlockingSpeaker: ISpeaker
    {
        public event MessageEvent Listeners;

        public virtual void SendMessage(string message)
        {
            if (this.Listeners != null)
            {
                this.Listeners.BeginInvoke(message, null, null);
            }
        }
    }
}
