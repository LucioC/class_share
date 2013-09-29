using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore
{
    public interface IWindowThreadControl
    {
        void StartThread();
        void StopThread();
        Boolean IsThreadRunning();
        CommandExecutor Executor { get; set; }
    }
}
