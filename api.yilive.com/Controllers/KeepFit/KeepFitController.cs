using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.yilive.com.Models;
using api.yilive.com.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tgnet.Api;
using Tgnet.Api.Mvc;
using YiLive.KeepFit;
using YiLive.User;
using Tgnet;
using System.Transactions;

namespace api.yilive.com.Controllers.KeepFit
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeepFitController : ControllerBase
    {
        private readonly IKitFitDiaryManager _KitFitDiaryManager;
        private readonly IUserServiceFactory _UserServiceFactory;
        private readonly IDailyFoodManager _DailyFoodManager;
        private readonly IKeepFitDiaryFactory _KeepFitDiaryFactory;
        public KeepFitController(IKitFitDiaryManager kitFitDiaryManager,
            IUserServiceFactory userServiceFactory,
            IDailyFoodManager dailyFoodManager,
            IKeepFitDiaryFactory keepFitDiaryFactory)
        {
            _KitFitDiaryManager = kitFitDiaryManager;
            _UserServiceFactory = userServiceFactory;
            _DailyFoodManager = dailyFoodManager;
            _KeepFitDiaryFactory = keepFitDiaryFactory;
        }
        [HttpGet]
        public object List(int?start,int? limit)
        {
            start = start ?? 0;
            start = start < 0 ? 0 : start;
            limit = limit ?? 10;
            limit = limit < 1 ? 10 : limit;
            var user = _UserServiceFactory.GetService(1);
            var diaries = _KitFitDiaryManager.GetDiaryList(user.Uid, start.Value, limit.Value);
            var kids = diaries.Select(p => p.Kid).ToArray();
            var foods = _DailyFoodManager.GetFoodsByKfIds(kids);
            var result = diaries.Select(p =>
            {
                var diaryFoods = foods.Where(f => f.KFId == p.Kid);
                var diary = new Models.KeepFitDiaryListItem(p, diaryFoods);
                return diary;
            }).ToArray();
            return this.JsonApiResult(ErrorCode.None, result);
        }

        [HttpPut]
        public object AddOrUpdate([FromBody]KeepFitDiaryUpdateRequest request)
        {
            ExceptionHelper.ThrowIfNull(request, nameof(request));
            var user = _UserServiceFactory.GetService(1);
            var diary = request.ConvertToDiaryUpdateModel();
            var foods = request.ConvertToFoodUpdateModel();
            using (var scope = new TransactionScope())
            {
                var kfid = _KitFitDiaryManager.AddOrUpdate(user, diary);
                _DailyFoodManager.AddOrUpdate(user,kfid,foods);
                scope.Complete();
            }
            return this.JsonApiResult(ErrorCode.None);
        }

        [HttpDelete]
        public object Delete ([FromQuery]long id)
        {
            var user = _UserServiceFactory.GetService(1);
            var service = _KeepFitDiaryFactory.GetService(user,id);
            service.Delete();
            return this.JsonApiResult(ErrorCode.None);
        }
    }
}