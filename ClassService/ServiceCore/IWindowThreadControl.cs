using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore
{
    public interface IWindowThreadControl: ISpeaker, IListener
    {
        void StartThread();
        void StopThread();
        Boolean IsThreadRunning();
    }
}
