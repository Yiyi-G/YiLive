using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Tgnet.Data;

namespace YiLive.Data.Repository
{
    public interface IKeepFitDiaryRepository : IRepository<KeepFitDiary>
    {

    }
    public class KeepFitDiaryRepository : BaseRepository<KeepFitDiary>, IKeepFitDiaryRepository
    {
        public KeepFitDiaryRepository(YiyiLiveContext context) : base(context)
        {
        }
    }
}
