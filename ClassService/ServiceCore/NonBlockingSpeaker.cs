using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore
{
    public class NonBlockingSpeaker: ISpeaker
    {
        public event MessageEvent MessageSent;

        public virtual void SendMessage(string message)
        {
            if (this.MessageSent != null)
            {
                this.MessageSent.BeginInvoke(message, null, null);
            }
        }
    }
}
