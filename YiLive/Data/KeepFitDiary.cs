using System;
using System.Collections.Generic;

namespace YiLive.Data
{
    public partial class KeepFitDiary
    {
        public long Id { get; set; }
        public long Uid { get; set; }
        public double? Weight { get; set; }
        public TimeSpan? MeasureTime { get; set; }
        public string Remark { get; set; }
        public DateTime Date { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }
        public bool IsEnabled { get; set; }
    }
}
