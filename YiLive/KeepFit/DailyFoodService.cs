using System;
using System.Collections.Generic;
using System.Text;
using YiLive.Data.Repository;
using YiLive.Data;
using System.Linq;
using Tgnet;
using Tgnet.Api;

namespace YiLive.KeepFit
{
    public interface IDailyFoodServiceFactory
    {
        IDailyFoodService GetService(long id, User.IUserService user);
    }
    public class DailyFoodServiceFactory : IDailyFoodServiceFactory
    {
        private readonly IDailyFoodRepository _DailyFoodRepository;
        public DailyFoodServiceFactory(long id, IDailyFoodRepository dailyFoodRepository)
        {
            _DailyFoodRepository = dailyFoodRepository;
        }
        public IDailyFoodService GetService(long id, User.IUserService user)
        {
            if (user != null)
                return new UserDailyFoodService(id, user, _DailyFoodRepository);
            else
                return new NoneUserDailyFoodService(id, _DailyFoodRepository);

        }
    }
    public interface IDailyFoodService
    {
        void Delete();
    }
    public abstract class DailyFoodService:IDailyFoodService
    {
        protected readonly long _Id;
        protected readonly Lazy<Data.DailyFood> _LazyEntity;
        protected readonly IDailyFoodRepository _DailyFoodRepository;
        public DailyFoodService(long id,IDailyFoodRepository dailyFoodRepository)
        {
            ExceptionHelper.ThrowIfNotId(id, nameof(id));
            _Id = id;
            _DailyFoodRepository = dailyFoodRepository;
            _LazyEntity = new Lazy<DailyFood>(() =>
            {
                var entity = _DailyFoodRepository.Entities.FirstOrDefault(p => p.Id == id);
                if (entity == null)
                    throw new ExceptionWithErrorCode(ErrorCode.没有找到对应条目);
                return entity;
            });
        }

        public abstract void Delete();
    }
    public class UserDailyFoodService :DailyFoodService, IDailyFoodService
    {
        User.IUserService _User;
        public UserDailyFoodService(
            long id,
            User.IUserService user,
            IDailyFoodRepository dailyFoodRepository)
            : base(id, dailyFoodRepository)
        {
            _User = user;
        }

        public override void Delete()
        {
            ThrowExceptionIfNotSelf();
            _LazyEntity.Value.IsEnabled = true;
            _DailyFoodRepository.SaveChanges();
        }

        private void ThrowExceptionIfNotSelf()
        {
            if (_LazyEntity.Value.Uid!=_User.Uid)
                throw new ExceptionWithErrorCode(ErrorCode.没有操作权限);
        }
    }
    public class NoneUserDailyFoodService : DailyFoodService, IDailyFoodService
    {
        public NoneUserDailyFoodService(
            long id,
            IDailyFoodRepository dailyFoodRepository)
            : base(id, dailyFoodRepository)
        {
        }

        public override void Delete()
        {
            ThrowExceptionIfNotAuthority();
        }

        private void ThrowExceptionIfNotAuthority()
        {
           throw new ExceptionWithErrorCode(ErrorCode.没有操作权限);
        }
    }
}
