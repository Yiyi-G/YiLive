using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Tgnet.Data;

namespace YiLive.Data.Repository
{
    public interface IDailyFoodRepository : IRepository<DailyFood>
    {

    }
    public class DailyFoodRepository : BaseRepository<DailyFood>, IDailyFoodRepository
    {
        public DailyFoodRepository(YiyiLiveContext context) : base(context)
        {
        }
    }
}
