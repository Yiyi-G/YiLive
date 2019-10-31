using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tgnet.Data.Entity;

namespace YiLive.Data.Repository
{
    public class BaseRepository<T> : DbSetRepository<DbContext, T> where T : class
    {
        private readonly DbContext _Context;
        private readonly DbSet<T> _DbSet;
        public BaseRepository(DbContext context) : base(context)
        {
            _Context = context;
            _DbSet = _Context.Set<T>();
        }
        protected override DbSet<T> DbSet
        {
            get { return _DbSet; }
        }
    }
}
