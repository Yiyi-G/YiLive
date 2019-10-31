using System;
using System.Collections.Generic;
using System.Text;

namespace YiLive.Models
{
    public class KeepFitDiaryUpdateModel
    {
        public long Kid { get; set; }
        public double? Weight { get; set; }
        public TimeSpan? MeasureTime { get; set; }
        public string Remark { get; set; }
        public DateTime Date { get; set; }
        public DiaryImgUpdateModel[] Imgs { get; set; }
    }
    public class DiaryImgUpdateModel
    {
        public string Url { get; set; }
        public int? Order { get; set; }
    }
}
