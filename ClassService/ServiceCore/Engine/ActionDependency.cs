using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Engine
{
    public class ActionDependency
    {
        public ActionType Type { get; protected set;}
        public ActionState State { get; protected set; }

        public ActionDependency(ActionType type)
        {
            Type = type;
            State = ActionState.WAITING;
        }
    }
}
