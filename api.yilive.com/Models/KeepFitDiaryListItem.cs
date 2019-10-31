using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using api.yilive.com.Utility;

namespace api.yilive.com.Models
{
    public class KeepFitDiaryListItem
    {
        [JsonProperty(PropertyName = "kid", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long Kid { get; set; }
        [JsonProperty(PropertyName = "weight", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? Weight { get; set; }
        [JsonProperty(PropertyName = "measure_time", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string MeasureTime { get; set; }
        [JsonProperty(PropertyName = "remark", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Remark { get; set; }
        [JsonProperty(PropertyName = "date", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Date { get; set; }
        [JsonProperty(PropertyName = "imgs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] Imgs { get; set; }
        [JsonProperty(PropertyName = "foods", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DailyFoodListItem[] Foods { get; set; }

        public KeepFitDiaryListItem() { }
        public KeepFitDiaryListItem(YiLive.Models.KeepFitDiaryModel model,IEnumerable<YiLive.Models.DailyFoodModel> foods)
        {
            Kid = model.Kid;
            Weight = model.Weight;
            MeasureTime = FieldConvertUtility.TimeSpanConvertToTime(model.MeasureTime);
            Remark = model.Remark;
            Date = model.Date.ToString("yyyy-MM-dd");
            Imgs = model.Imgs;
            if (foods != null)
            {
                Foods = foods.Select(p => new Models.DailyFoodListItem(p)).ToArray();
            }
        }
    }
}
