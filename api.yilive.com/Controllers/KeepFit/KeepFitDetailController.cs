using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tgnet.Api;
using Tgnet.Api.Mvc;
using YiLive.KeepFit;
using YiLive.User;

namespace api.yilive.com.Controllers.KeepFit
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeepFitDetailController : ControllerBase
    {
        private readonly IKitFitDiaryManager _KitFitDiaryManager;
        private readonly IUserServiceFactory _UserServiceFactory;
        private readonly IDailyFoodManager _DailyFoodManager;
        public KeepFitDetailController(IKitFitDiaryManager kitFitDiaryManager,
            IUserServiceFactory userServiceFactory,
            IDailyFoodManager dailyFoodManager)
        {
            _KitFitDiaryManager = kitFitDiaryManager;
            _UserServiceFactory = userServiceFactory;
            _DailyFoodManager = dailyFoodManager;
        }
        [HttpGet]
        public object Detail(long id)
        {
            var user = _UserServiceFactory.GetService(1);
            int count = 0;
            var diary = _KitFitDiaryManager.GetDiaryList(user.Uid, 0, 1, out count).FirstOrDefault();
            var foods = _DailyFoodManager.GetFoodsByKfIds(new long[diary.Kid]);
            var result = new Models.KeepFitDiaryListItem(diary, foods)
            {
                Num = 0
            };
            return this.JsonApiResult(ErrorCode.None, result);
        }
    }
}