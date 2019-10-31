using System;
using System.Collections.Generic;
using System.Text;

namespace YiLive.Models
{
    public class KeepFitDiaryModel
    {
        public long Kid { get; set; }
        public long Uid { get; set; }
        public double? Weight { get; set; }
        public TimeSpan? MeasureTime { get; set; }
        public string Remark { get; set; }
        public DateTime Date { get; set; }
        public string[] Imgs { get; set; }
    }
}
