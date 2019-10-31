using System;
using System.Collections.Generic;

namespace YiLive.Data
{
    public partial class DailyFood
    {
        public long Id { get; set; }
        public long Kfid { get; set; }
        public long Uid { get; set; }
        public string Title { get; set; }
        public string Foods { get; set; }
        public bool IsMain { get; set; }
        public TimeSpan EatTime { get; set; }
        public DateTime Updated { get; set; }
        public bool IsEnabled { get; set; }
    }
}
