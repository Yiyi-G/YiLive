using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Tgnet.Data;

namespace YiLive.Data.Repository
{
    public interface IKeepFitDiaryImgRepository : IRepository<KeepFitDiaryImg>
    {

    }
    public class KeepFitDiaryImgRepository : BaseRepository<KeepFitDiaryImg>, IKeepFitDiaryImgRepository
    {
        public KeepFitDiaryImgRepository(YiyiLiveContext context) : base(context)
        {
        }
    }
}
