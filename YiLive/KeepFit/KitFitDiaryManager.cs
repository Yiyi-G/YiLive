using System;
using System.Collections.Generic;
using System.Text;
using YiLive.User;
using YiLive.Models;
using YiLive.Data.Repository;
using Tgnet;
using Tgnet.Api;
using System.Linq;
using System.Transactions;
using Tgnet.Linq;

namespace YiLive.KeepFit
{
    public interface IKitFitDiaryManager
    {
        long AddOrUpdate(IUserService user, KeepFitDiaryUpdateModel model);
        Models.KeepFitDiaryModel[] GetDiaryList(long uid, int start, int limit, out int count);
    }
    public class KitFitDiaryManager : IKitFitDiaryManager
    {
        private readonly IKeepFitDiaryRepository _KeepFitDiaryRepository;
        private readonly IKeepFitDiaryImgRepository _KeepFitDiaryImgRepository;
        public KitFitDiaryManager(
            IKeepFitDiaryRepository keepFitDiaryRepository,
            IKeepFitDiaryImgRepository keepFitDiaryImgRepository)
        {
            _KeepFitDiaryRepository = keepFitDiaryRepository;
            _KeepFitDiaryImgRepository = keepFitDiaryImgRepository;
        }
        public long AddOrUpdate(IUserService user, KeepFitDiaryUpdateModel model)
        {
            ExceptionHelper.ThrowIfNull(user, nameof(user));
            ExceptionHelper.ThrowIfNull(model, nameof(model));
            Data.KeepFitDiary entity = null;
            var uid = user.Uid;
            var now = DateTime.Now;
            if (model.Kid > 0)
            {
                entity = _KeepFitDiaryRepository.Entities.FirstOrDefault(p => p.IsEnabled && p.Id == model.Kid);
                if (entity != null && entity.Uid != uid)
                    throw new ExceptionWithErrorCode(ErrorCode.没有操作权限, "只能修改本人的日记");
            }
            using (var scope = new TransactionScope())
            {
                if (entity != null)
                {
                    entity.Date = model.Date;
                    entity.MeasureTime = model.MeasureTime;
                    entity.Weight = model.Weight;
                    entity.Updated = now;
                    if (model.Remark != null)
                        entity.Remark = model.Remark;
                }
                else
                {
                    entity= _KeepFitDiaryRepository.Add(new Data.KeepFitDiary()
                    {
                        Uid = uid,
                        Weight = model.Weight,
                        Remark = model.Remark,
                        Date = model.Date,
                        MeasureTime = model.MeasureTime,
                        Created = now,
                        Updated = now,
                        IsEnabled = true
                    });
                }
                _KeepFitDiaryRepository.SaveChanges();
                var kfid = entity.Id;
                if (model.Imgs != null)
                {
                    model.Imgs = model.Imgs.Where(p => !string.IsNullOrWhiteSpace(p.Url)).Distinct().ToArray();
                    var existImgs = _KeepFitDiaryImgRepository.Entities.Where(p => p.Kfid == kfid && p.IsEnabled).ToArray();
                    _KeepFitDiaryImgRepository.AddRange(model.Imgs.Where(p => !existImgs.Any(e => e.DirUrl == p.Url))
                        .Select(p => new Data.KeepFitDiaryImg()
                        {
                            Kfid = kfid,
                            DirUrl = p.Url,
                            Updated = now,
                            IsEnabled = true,
                        }));
                    var needDeleteImgs = existImgs.Where(p => !model.Imgs.Any(i => i.Url == p.DirUrl));
                    _KeepFitDiaryImgRepository.DeleteRange(needDeleteImgs);
                    var updateImgs = existImgs.Except(needDeleteImgs);
                    foreach (var uImg in updateImgs)
                    {
                        uImg.Order = model.Imgs.First(p => p.Url == uImg.DirUrl).Order;
                    }
                    _KeepFitDiaryImgRepository.SaveChanges();
                }

                scope.Complete();
            }
            return entity.Id;
        }

        public  KeepFitDiaryModel[] GetDiaryList(long uid, int start, int limit,out int count)
        {
            var query = _KeepFitDiaryRepository.Entities.Where(p => p.Uid == uid && p.IsEnabled);
            count = query.Count();
            var diaries = query
                 .OrderByDescending(p => p.Date).Take(start, limit)
                 .Select(p => new KeepFitDiaryModel()
                 {
                     Kid = p.Id,
                     Remark = p.Remark,
                     Weight = p.Weight,
                     Uid = p.Uid,
                     Date = p.Date,
                     MeasureTime = p.MeasureTime,
                 }).ToArray();

            var kids = diaries.Select(p => p.Kid).ToArray();
            var imgs = _KeepFitDiaryImgRepository.Entities.Where(f => kids.Contains(f.Kfid)&&f.IsEnabled).ToArray();
            foreach (var diary in diaries)
            {
                diary.Imgs = imgs.Where(p => p.Kfid == diary.Kid).OrderBy(p => p.Order).Select(p => p.DirUrl).ToArray();
            }
            return diaries;
        }
    }
}
