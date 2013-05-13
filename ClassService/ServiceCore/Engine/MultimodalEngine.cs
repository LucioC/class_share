using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Engine
{
    public class MultimodalEngine
    {
        public List<IModalityAction> actions { get; protected set;}

        public MultimodalEngine()
        {
            actions = new List<IModalityAction>();
        }

        public void NewEvent(IModalityAction modalityAction)
        {
            actions.Add(modalityAction);
        }

    }
}
