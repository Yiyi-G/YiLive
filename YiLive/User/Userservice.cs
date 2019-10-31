using System;
using System.Collections.Generic;
using System.Text;
using Tgnet;
using Tgnet.Api;

namespace YiLive.User
{
    public interface IUserServiceFactory
    {
        IUserService GetService(long uid);
    }
    public class UserServiceFactory : IUserServiceFactory
    {
        public IUserService GetService(long uid)
        {
            return new UserService(uid);
        }
    }
    public interface IUserService
    {
        long Uid { get; }
    }
    public  class UserService: IUserService
    {
        private readonly long _Uid;
        public UserService(long uid)
        {
            ExceptionHelper.ThrowIfNotId(uid, nameof(uid));
            _Uid = uid;
        }

        public long Uid => _Uid;
    }
}
