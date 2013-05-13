using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Engine
{
    public interface IModalityAction
    {
        ActionType Type { get; }
    }
}
