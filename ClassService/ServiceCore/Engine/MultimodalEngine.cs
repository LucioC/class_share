using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceCore.Engine.Triggers;
using ServiceCore.Utils;

namespace ServiceCore.Engine
{
    public class MultimodalEngine
    {
        public List<ModalityEvent> Actions { get; protected set;}
        public List<EffectTrigger> Triggers { get; protected set; }
        public Clock ClockInstance { get; set; }

        public MultimodalEngine()
        {
            Actions = new List<ModalityEvent>();
            Triggers = new List<EffectTrigger>();
            ClockInstance = new Clock();
        }

        public void NewInputModalityEvent(ModalityEvent modalityAction)
        {
            Actions.Add(modalityAction);

            List<EffectTrigger> matchingTriggers = SearchTriggers(modalityAction);
            TriggerEffects(matchingTriggers);
        }

        private static void TriggerEffects(List<EffectTrigger> matchingTriggers)
        {
            foreach (var trigger in matchingTriggers)
            {
                foreach (var effect in trigger.Effects)
                {
                    effect.execute();
                }
            }
        }

        public List<EffectTrigger> SearchTriggers(ModalityEvent newModalityEvent)
        {
            List<EffectTrigger> results = new List<EffectTrigger>();

            foreach (var trigger in Triggers)
            {
                foreach (var modality in trigger.Triggers)
                {
                    if (trigger.HasEvent(modality))
                    {
                        trigger.SetNewEvent(newModalityEvent);

                        if(trigger.IsReadyToTrigger(ClockInstance.CurrentTimeInMillis()))
                        {
                            results.Add(trigger);
                        }
                    }
                }
            }

            return results;
        }

        public void addNewTrigger(EffectTrigger effectTrigger)
        {
            Triggers.Add(effectTrigger);
        }
    }
}
