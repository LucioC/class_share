using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Engine
{
    public class EffectTrigger
    {
        public List<ModalityEvent> Triggers { get; set; }
        public List<IEffect> Effects { get; set; }

        public EffectTrigger()
        {
            Triggers = new List<ModalityEvent>();
            Effects = new List<IEffect>();
        }

        public Boolean HasEvent(ModalityEvent modalityEvent)
        {
            foreach(var trigger in Triggers)
            {
                if (modalityEvent.Type == trigger.Type)
                {
                    return true;
                }
            }
            return false;
        }

        public void SetNewEvent(ModalityEvent modalityEvent)
        {
            foreach (var trigger in Triggers)
            {
                if (modalityEvent.Type == trigger.Type)
                {
                    trigger.State = ActionState.HAPPENED;
                }
            }
        }

        public void clearStates()
        {
            foreach (var trigger in Triggers)
            {
                trigger.State = ActionState.WAITING;
            }
        }

        public Boolean IsReadyToTrigger()
        {
            foreach(var trigger in Triggers)
            {
                if (trigger.State == ActionState.WAITING)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
