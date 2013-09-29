using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore
{
    public delegate void MessageEvent(string message);
    public interface ISpeaker
    {
        event MessageEvent Listeners;
        void SendMessage(string message);
    }
}
