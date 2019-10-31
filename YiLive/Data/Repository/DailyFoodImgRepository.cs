using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Tgnet.Data;

namespace YiLive.Data.Repository
{
    public interface IDailyFoodImgRepository : IRepository<DailyFoodImg>
    {

    }
    public class DailyFoodImgRepository : BaseRepository<DailyFoodImg>, IDailyFoodImgRepository
    {
        public DailyFoodImgRepository(YiyiLiveContext context) : base(context)
        {
        }
    }
}
