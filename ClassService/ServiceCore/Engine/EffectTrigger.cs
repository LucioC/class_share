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
    }
}
