using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.yilive.com.Models.Request
{
    public class KeepFitDiaryUpdateRequest
    {
        public long kid { get; set; }
        public double? weight { get; set; }
        public TimeSpan? measure_time { get; set; }
        public string remark { get; set; }
        public DateTime date { get; set; }
        public string[] imgs { get; set; }
        public DailyFoodUpdateRequest[] foods { get; set;}
        public YiLive.Models.KeepFitDiaryUpdateModel ConvertToDiaryUpdateModel()
        {
            var model = new YiLive.Models.KeepFitDiaryUpdateModel();
            model.Kid = kid;
            model.Weight = weight;
            model.Remark = remark;
            model.MeasureTime = Utility.FieldConvertUtility.TimeSpanConvertToLessDay(measure_time.Value);
            model.Date = date;
            if (imgs != null)
            {
                int i = 0;
                model.Imgs = imgs.Select(p => new YiLive.Models.DiaryImgUpdateModel()
                {
                    Order = i++,
                    Url = Utility.FileUtility.SaveTempleFile(p,"keepFit/Diary/")
                }).ToArray();
            }
            return model;
        }
        public YiLive.Models.DailyFoodUpdateModel[] ConvertToFoodUpdateModel()
        {
            YiLive.Models.DailyFoodUpdateModel[] foodModels = null;
            if (foods != null)
            {
                foodModels = foods.Where(p => p != null).Select(p => p.ConvertToFoodUpdateModel()).ToArray();
            }
            return foodModels;
        }
    }

    public class DailyFoodUpdateRequest
    {
        public long id { get; set; }
        public string title { get; set; }
        public string foods { get; set; }
        public bool is_main { get; set; }
        public TimeSpan eat_time { get; set; }
        public string [] imgs { get; set; }
        
        public YiLive.Models.DailyFoodUpdateModel ConvertToFoodUpdateModel()
        {
            var model = new YiLive.Models.DailyFoodUpdateModel();
            model.Id = id;
            model.Title = title;
            model.EatTime = Utility.FieldConvertUtility.TimeSpanConvertToLessDay(eat_time);
            model.Foods = foods;
            model.IsMain = is_main;
            if (imgs != null)
            {
                int i = 0;
                model.Imgs = imgs.Select(p => new YiLive.Models.FoodImgUpdateModel()
                {
                    Order = i++,
                    Url = Utility.FileUtility.SaveTempleFile(p, "keepFit/Food/")
                }).ToArray();
            }
            return model;
        }
    }

   
}
