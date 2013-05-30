using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtils;

namespace KinectPowerPointControl.Gesture
{
    public class IntervalControl
    {
        protected long lastTrigged = 0;
        protected Time time = new Time();

        public long Interval { get; set; }

        public IntervalControl()
        {
            Interval = 0;
        }

        public bool HasIntervalPassed()
        {
            long now = time.CurrentTimeInMillis();
            if (now - lastTrigged > Interval)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void TriggerIt()
        {
            long now = time.CurrentTimeInMillis();
            lastTrigged = now;
        }
    }
}
