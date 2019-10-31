using System;
using System.Collections.Generic;
using System.Text;

namespace YiLive.Models
{
    public class DailyFoodUpdateModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Foods { get; set; }
        public bool IsMain { get; set; }
        public TimeSpan EatTime { get; set; }
        public FoodImgUpdateModel[] Imgs { get; set; }
    }
    public class FoodImgUpdateModel
    {
        public string Url { get; set; }
        public int? Order { get; set; }
    }
}
