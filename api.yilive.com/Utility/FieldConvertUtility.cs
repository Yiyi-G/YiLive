using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.yilive.com.Utility
{
    public static class FieldConvertUtility
    {
        public static string TimeSpanConvertToTime(TimeSpan? span)
        {
            var result = "--";
            if (span.HasValue)
            {
                result = span.Value.ToString("hh\\:ss");
            }
            return result;
        }

        public static TimeSpan TimeSpanConvertToLessDay(TimeSpan span)
        {
            var millisecondesPerDay = 86400000;
            if (span.TotalMilliseconds > millisecondesPerDay)
            {
                var millisecondes = (long)span.TotalMilliseconds;
                if (millisecondes > millisecondesPerDay)
                {
                    millisecondes = millisecondes % millisecondesPerDay;
                    span = new TimeSpan(millisecondes * TimeSpan.TicksPerMillisecond);
                }
            }
            return span;
        }
    }
}
