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
    public class DailyFoodController : ControllerBase
    {
        private readonly IKeepFitDiaryFactory _KeepFitDiaryFactory;
        private readonly IDailyFoodServiceFactory _DailyFoodServiceFactory;
        private readonly IUserServiceFactory _UserServiceFactory;
        public DailyFoodController(
            IKeepFitDiaryFactory keepFitDiaryFactory,
            IDailyFoodServiceFactory dailyFoodServiceFactory,
            IUserServiceFactory userServiceFactory
            )
        {
            _KeepFitDiaryFactory = keepFitDiaryFactory;
            _DailyFoodServiceFactory = dailyFoodServiceFactory;
            _UserServiceFactory = userServiceFactory;
        }

        [HttpDelete]
        public object Delete([FromQuery]long id)
        {
            var user = _UserServiceFactory.GetService(1);
            var service = _DailyFoodServiceFactory.GetService(id, user);
            service.Delete();
            return this.JsonApiResult(ErrorCode.None);
        }
    }
}