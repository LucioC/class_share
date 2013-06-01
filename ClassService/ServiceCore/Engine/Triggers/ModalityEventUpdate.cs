using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Engine.Triggers
{
    public class ModalityEventParallelControl
    {
        public static Boolean IsReadyToTrigger(List<ModalityEvent> events, long timeWindows)
        {
            List<ModalityEvent> eventsCopy = new List<ModalityEvent>(events);
            eventsCopy.OrderBy(x => x.EventTime).ToList();

            long largestDifferenceTime = Math.Abs(eventsCopy.First().EventTime - eventsCopy.Last().EventTime);

            if (largestDifferenceTime > timeWindows)
            {
                return false;
            }
            else return true;
        }

        public static void UpdateEventsState(List<ModalityEvent> events, long timeWindows, long currentTime)
        {
            if (timeWindows == 0) return;
            foreach (var modalityEvent in events)
            {
                long passed = currentTime - modalityEvent.EventTime;
                if (passed > timeWindows)
                {
                    modalityEvent.State = ActionState.WAITING;
                    modalityEvent.EventTime = 0;
                }
            }
        }
    }
}
