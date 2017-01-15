using OneDo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OneDo.Data.Repositories
{
    public interface IQueryRepository<TEntity> where TEntity : class, IEntity
    {
        Task<IList<TEntity>> GetAll();

        Task<IList<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> Get(Guid id);

        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        Task<bool> Any();

        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);

        Task<int> Count();

        Task<int> Count(Expression<Func<TEntity, bool>> predicate);
    }
}