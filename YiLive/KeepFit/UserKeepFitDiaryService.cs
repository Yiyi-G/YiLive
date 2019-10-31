using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tgnet;
using YiLive.Data.Repository;
using Tgnet.Api;

namespace YiLive.KeepFit
{
    public interface IKeepFitDiaryFactory
    {
        IUserKeepFitDiaryService GetService(User.IUserService user, long kid);
    }
    public class KeepFitDiaryFactory : IKeepFitDiaryFactory
    {
        private readonly IKeepFitDiaryRepository _KeepFitDiaryRepository;
        private readonly IKeepFitDiaryImgRepository _KeepFitDiaryImgRepository;
        public KeepFitDiaryFactory(
            IKeepFitDiaryRepository keepFitDiaryRepository,
            IKeepFitDiaryImgRepository keepFitDiaryImgRepository)
        {
            _KeepFitDiaryRepository = keepFitDiaryRepository;
            _KeepFitDiaryImgRepository = keepFitDiaryImgRepository;
        }
        public IUserKeepFitDiaryService GetService(User.IUserService user, long kid)
        {
            if (user != null)
                return new UserKeepFitDiaryService(kid, user, _KeepFitDiaryRepository, _KeepFitDiaryImgRepository);
            else
                return new NoneUserKeepFitDiaryService(kid, _KeepFitDiaryRepository, _KeepFitDiaryImgRepository);
        }
    }

    public interface IUserKeepFitDiaryService
    {
        long Kid { get; }
        long Uid { get; }
        double? Weight { get; }
        TimeSpan? MeasureTime { get; }
        string Remark { get; }
        DateTime Date { get; }
        string[] DiaryImg { get; }
        void Delete();
        bool CheckUpdateAuthority { get; }

    }

    public abstract class BaseKeepFitDiaryService : IUserKeepFitDiaryService
    {
        protected readonly long _Kid;
        protected readonly Lazy<Data.KeepFitDiary> _LazyEntity;
        protected readonly IKeepFitDiaryRepository _KeepFitDiaryRepository;
        protected readonly IKeepFitDiaryImgRepository _KeepFitDiaryImgRepository;
        public BaseKeepFitDiaryService(long kid,
            IKeepFitDiaryRepository keepFitDiaryRepository,
            IKeepFitDiaryImgRepository keepFitDiaryImgRepository)
        {
            ExceptionHelper.ThrowIfNotId(kid, nameof(kid));
            _Kid = kid;
            _KeepFitDiaryRepository = keepFitDiaryRepository;
            _KeepFitDiaryImgRepository = keepFitDiaryImgRepository;
            _LazyEntity = new Lazy<Data.KeepFitDiary>(() =>
            {
                var entity = _KeepFitDiaryRepository.Entities.FirstOrDefault(p => p.Id == kid);
                if (entity == null)
                    throw new ExceptionWithErrorCode(ErrorCode.没有找到对应条目);
                return entity;
            }
            );
        }
        public long Kid => _LazyEntity.Value.Id;

        public long Uid => _LazyEntity.Value.Uid;

        public double? Weight => _LazyEntity.Value.Weight;

        public TimeSpan? MeasureTime => _LazyEntity.Value.MeasureTime;

        public string Remark => _LazyEntity.Value.Remark;

        public DateTime Date => _LazyEntity.Value.Date;

        public string[] DiaryImg
        {
            get
            {
                return _KeepFitDiaryImgRepository.Entities.Where(p => p.Kfid == _Kid && p.IsEnabled).OrderBy(p => p.Order).Select(p => p.DirUrl).ToArray();
            }
        }

        public abstract bool CheckUpdateAuthority { get; }

        public abstract void Delete();
    }
    public class UserKeepFitDiaryService :BaseKeepFitDiaryService,IUserKeepFitDiaryService
    {
        private readonly User.IUserService _User;
        public UserKeepFitDiaryService(long kid,User.IUserService user,
            IKeepFitDiaryRepository keepFitDiaryRepository,
            IKeepFitDiaryImgRepository keepFitDiaryImgRepository)
            :base(kid,keepFitDiaryRepository,keepFitDiaryImgRepository)
        {
            _User = user;
        }

        public override bool CheckUpdateAuthority => CheckIsSelf();

        public override void Delete()
        {
            ThrowExceptionIfNotSelf();
            _LazyEntity.Value.IsEnabled = true;
            _KeepFitDiaryRepository.SaveChanges();
        }
        private bool CheckIsSelf()
        {
            return _LazyEntity.Value.Uid == _User.Uid;
        }
        private void ThrowExceptionIfNotSelf()
        {
            if (!CheckIsSelf())
                throw new ExceptionWithErrorCode(ErrorCode.没有操作权限);
        }

    }
    public class NoneUserKeepFitDiaryService : BaseKeepFitDiaryService, IUserKeepFitDiaryService
    {
        public NoneUserKeepFitDiaryService(long kid,
            IKeepFitDiaryRepository keepFitDiaryRepository,
            IKeepFitDiaryImgRepository keepFitDiaryImgRepository)
            : base(kid, keepFitDiaryRepository, keepFitDiaryImgRepository)
        {
        }

        public override bool CheckUpdateAuthority => false;

        public override void Delete()
        {
            throw new ExceptionWithErrorCode(ErrorCode.没有操作权限);
        }
    }
}
