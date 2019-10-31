using System;
using System.Collections.Generic;
using System.Text;
using YiLive.User;
using YiLive.Models;
using YiLive.Data.Repository;
using System.Transactions;
using Tgnet;
using System.Linq;

namespace YiLive.KeepFit
{
    public interface IDailyFoodManager
    {
        void AddOrUpdate(IUserService user, long kfid, DailyFoodUpdateModel[] models);
        DailyFoodModel[] GetFoodsByKfIds(long[] kfids);
    }
    public class DailyFoodManager : IDailyFoodManager
    {
        private readonly IDailyFoodRepository _DailyFoodRepository;
        private readonly IDailyFoodImgRepository _DailyFoodImgRepository;
        public DailyFoodManager(
            IDailyFoodRepository dailyFoodRepository,
            IDailyFoodImgRepository dailyFoodImgRepository
            )
        {
            _DailyFoodRepository = dailyFoodRepository;
            _DailyFoodImgRepository = dailyFoodImgRepository;
        }
        public void AddOrUpdate(IUserService user, long kfid, DailyFoodUpdateModel[] models)
        {
            ExceptionHelper.ThrowIfNotId(kfid, nameof(kfid));
            ExceptionHelper.ThrowIfNull(models, nameof(models));
            var uid = user.Uid;
            var ids = models.Where(p => p.Id > 0).Select(p => p.Id).Distinct().ToArray();
            var now = DateTime.Now;
            var dailyFoodImgMaps = new List<DailyFoodImgMap>();
            Data.DailyFood[] existEntitys;
            Data.DailyFoodImg[] existImgs;
            //using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
                existEntitys = _DailyFoodRepository.Entities.Where(p =>p.Uid==uid&&ids.Contains(p.Id) && p.Kfid == kfid && p.IsEnabled).ToArray();
                var existDfids = existEntitys.Select(p => p.Id).ToArray();
                existImgs = _DailyFoodImgRepository.Entities.Where(p => existDfids.Contains(p.Dfid) && p.IsEnabled).ToArray();
            //    scope.Complete();
            //}
            using (var scope = new TransactionScope())
            {
                foreach (var model in models)
                {
                    var entity = existEntitys.FirstOrDefault(p => p.Id == model.Id);
                    if (entity != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.Title))
                            entity.Title = model.Title;
                        if (model.Foods != null)
                            entity.Foods = model.Foods;
                        entity.IsMain = model.IsMain;
                        entity.EatTime = model.EatTime;
                        entity.Updated = now;
                    }
                    else
                    {
                        entity = new Data.DailyFood()
                        {
                            Kfid = kfid,
                            Uid = uid,
                            Title = model.Title,
                            Foods = model.Foods,
                            IsMain = model.IsMain,
                            EatTime = model.EatTime,
                            Updated = now,
                            IsEnabled = true
                        };
                        _DailyFoodRepository.Add(entity);
                    }
                    dailyFoodImgMaps.Add(new DailyFoodImgMap(entity, model.Imgs));
                }
                _DailyFoodRepository.SaveChanges();
                AddOrUpdateImg(dailyFoodImgMaps, existImgs);
                scope.Complete();
            }
        }

        public DailyFoodModel[] GetFoodsByKfIds(long[] kfids)
        {
            kfids = (kfids ?? new long[0]).Where(p => p > 0).Distinct().ToArray();
            if (kfids.Length == 0) return new DailyFoodModel[0];
            var foods = _DailyFoodRepository.Entities.Where(p => kfids.Contains(p.Kfid) && p.IsEnabled)
                 .Select(p => new DailyFoodModel()
                 {
                     Id = p.Id,
                     KFId = p.Kfid,
                     EatTime = p.EatTime,
                     Foods = p.Foods,
                     IsMain = p.IsMain,
                     Title = p.Title,
                 }).ToArray();

            var fids = foods.Select(p => p.Id).ToArray();
            var imgs = _DailyFoodImgRepository.Entities.Where(f => fids.Contains(f.Dfid)&&f.IsEnabled).ToArray();
            foreach (var food in foods)
            {
                food.Imgs = imgs.Where(p => p.Dfid == food.Id).OrderBy(p => p.Order).Select(p => p.DirUrl).ToArray();
            }
            return foods;
        }

        private void AddOrUpdateImg(List<DailyFoodImgMap> maps, Data.DailyFoodImg[] existImgs)
        {
            if (maps != null)
            {
                var now = DateTime.Now;
                foreach (var map in maps)
                {
                    //修改图片
                    if (map.Imgs != null)
                    {
                        var entity = map.DailyFood;
                        map.Imgs = map.Imgs.Where(p => !string.IsNullOrWhiteSpace(p.Url)).Distinct().ToArray();
                        var imgs = existImgs.Where(p => p.Dfid == entity.Id).ToArray();
                        _DailyFoodImgRepository.AddRange(map.Imgs.Where(p => !imgs.Any(i => i.DirUrl == p.Url))
                            .Select(p => new Data.DailyFoodImg()
                            {
                                Dfid = entity.Id,
                                DirUrl = p.Url,
                                Order = p.Order,
                                IsEnabled = true,
                                Updated = now
                            }));
                        var needDeleteImgs = imgs.Where(p => !map.Imgs.Any(i => i.Url == p.DirUrl));
                        foreach (var dImg in needDeleteImgs)
                        {
                            dImg.IsEnabled = false;
                        }
                        var updateImgs = existImgs.Except(needDeleteImgs);
                        foreach (var uImg in updateImgs)
                        {
                            uImg.Order = map.Imgs.First(p => p.Url == uImg.DirUrl).Order;
                        }
                    }

                }
                _DailyFoodImgRepository.SaveChanges();
            }
        }
        public class DailyFoodImgMap
        {
            public Data.DailyFood DailyFood { get; set; }
            public FoodImgUpdateModel[] Imgs { get; set; }
            public DailyFoodImgMap(Data.DailyFood food, FoodImgUpdateModel[] imgs)
            {
                DailyFood = food;
                Imgs = imgs;
            }
        }
    }


}
