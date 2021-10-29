using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCore.Sample.Business.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAll();

        Task Add(TEntity entity);

        Task Update(TEntity entity);

    }
}
