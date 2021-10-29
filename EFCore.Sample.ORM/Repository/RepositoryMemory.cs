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
    public abstract class RepositoryMemory<TEntity> : IRepository<TEntity> where TEntity : class, new() 
    {
        protected readonly Sped_SafewebContextMemory Db;
        protected readonly DbSet<TEntity> DbSet;

        protected RepositoryMemory(Sped_SafewebContextMemory db)
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

        public async Task Add(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
        }

        public async Task Update(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChanges();

        }

        public async Task<int> SaveChanges() {
            return await Db.SaveChangesAsync();
        }
    }
}
