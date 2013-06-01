﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Engine
{
    public class ModalityEvent
    {
        public ActionType Type { get; set; }
        public ActionState State { get; set; }
        public long EventTime { get; set; }

        public ModalityEvent()
        {
            State = ActionState.WAITING;
        }

    }
}
