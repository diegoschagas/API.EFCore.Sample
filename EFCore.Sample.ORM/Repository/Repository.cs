using Microsoft.EntityFrameworkCore;
using EFCore.Sample.Business.Interfaces;
using EFCore.Sample.ORM.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCore.Sample.ORM.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly Sped_SafewebContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(Sped_SafewebContext db)
        {
            Db = db;
            DbSet = Db.Set<TEntity>();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }

        public async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public Task<TEntity> Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        Task IRepository<TEntity>.Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        Task IRepository<TEntity>.Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
