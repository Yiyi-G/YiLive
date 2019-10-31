using System;
using System.Collections.Generic;

namespace YiLive.Data
{
    public partial class DailyFoodImg
    {
        public long Id { get; set; }
        public long Dfid { get; set; }
        public string DirUrl { get; set; }
        public int? Order { get; set; }
        public DateTime Updated { get; set; }
        public bool IsEnabled { get; set; }
    }
}
