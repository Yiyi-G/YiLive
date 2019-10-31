using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using YiLive.KeepFit;
using YiLive.User;

namespace YiLive.Inject
{
     public static class ServiceInject
    {
        public static void BindServiceInject(IServiceCollection services)
        {
            services.AddScoped<IKeepFitDiaryFactory , KeepFitDiaryFactory>();
            services.AddScoped<IKitFitDiaryManager, KitFitDiaryManager>();
            services.AddScoped<IDailyFoodManager, DailyFoodManager>();
            services.AddScoped<IUserServiceFactory, UserServiceFactory>();
            services.AddScoped<IDailyFoodServiceFactory, DailyFoodServiceFactory>();
        }
    }
}
