using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore.Utils
{
    public class Clock
    {
        public Clock()
        {

        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public virtual long CurrentTimeInMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
    }
}
