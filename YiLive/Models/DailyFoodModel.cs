using System;
using System.Collections.Generic;
using System.Text;

namespace YiLive.Models
{
    public class DailyFoodModel
    {
        public long Id { get; set; }
        public long KFId { get; set;}
        public string Title { get; set; }
        public string Foods { get; set; }
        public bool IsMain { get; set; }
        public TimeSpan EatTime { get; set; }
        public string[] Imgs { get; set; }

    }
}
