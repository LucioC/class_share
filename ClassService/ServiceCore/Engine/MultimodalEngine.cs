using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Engine
{
    public class MultimodalEngine
    {
        public List<ModalityEvent> Actions { get; protected set;}
        public List<EffectTrigger> Triggers { get; protected set; }

        public MultimodalEngine()
        {
            Actions = new List<ModalityEvent>();
            Triggers = new List<EffectTrigger>();
        }

        public void NewInputModalityEvent(ModalityEvent modalityAction)
        {
            Actions.Add(modalityAction);
            Triggers[0].Effects[0].execute();
        }

        public List<EffectTrigger> SearchTriggers(ModalityEvent modalityEvent)
        {
            List<EffectTrigger> results = null;

            //results = Triggers.Where();

            return results;
        }

        public void addNewTrigger(EffectTrigger effectTrigger)
        {
            Triggers.Add(effectTrigger);
        }
    }
}
