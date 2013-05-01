using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore
{
    public delegate void MessageEvent(string message);
    public interface IWindowThreadControl
    {
        void StartThread();
        void StopThread();
        Boolean IsThreadRunning();
        event MessageEvent MessageSent;
    }
}
