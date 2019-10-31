using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using YiLive.Data.Repository;

namespace YiLive.Inject
{
    public static class DataInject
    {
        public static void BindDataInject(IServiceCollection services)
        {
            services.AddScoped<IKeepFitDiaryRepository, KeepFitDiaryRepository>();
            services.AddScoped<IKeepFitDiaryImgRepository, KeepFitDiaryImgRepository>();
            services.AddScoped<IDailyFoodRepository, DailyFoodRepository>();
            services.AddScoped<IDailyFoodImgRepository, DailyFoodImgRepository>();
        }
    }
}
