using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.yilive.com.Models
{
    public class DailyFoodListItem
    {
        [JsonProperty(PropertyName = "id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long Id { get; set; }
        [JsonProperty(PropertyName = "kfid", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long KFId { get; set; }
        [JsonProperty(PropertyName = "title", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "foods", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Foods { get; set; }
        [JsonProperty(PropertyName = "is_main", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsMain { get; set; }
        [JsonProperty(PropertyName = "eat_time", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string EatTime { get; set; }
        [JsonProperty(PropertyName = "imgs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] Imgs { get; set; }
        public DailyFoodListItem() { }
        public DailyFoodListItem(YiLive.Models.DailyFoodModel model)
        {
            Id = model.Id;
            KFId = model.KFId;
            Title = model.Title;
            Foods = model.Foods;
            IsMain = model.IsMain;
            EatTime =Utility.FieldConvertUtility.TimeSpanConvertToTime(model.EatTime);
            Imgs = model.Imgs;
        }
    }
}
