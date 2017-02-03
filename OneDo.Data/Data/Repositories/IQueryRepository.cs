using OneDo.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OneDo.Infrastructure.Data.Repositories
{
    public interface IQueryRepository<TEntityData> where TEntityData : class, IEntityData
    {
        Task<IList<TEntityData>> GetAll();

        Task<IList<TEntityData>> GetAll(Expression<Func<TEntityData, bool>> predicate);

        Task<TEntityData> Get(Guid id);

        Task<TEntityData> Get(Expression<Func<TEntityData, bool>> predicate);

        Task<bool> Any();

        Task<bool> Any(Expression<Func<TEntityData, bool>> predicate);

        Task<int> Count();

        Task<int> Count(Expression<Func<TEntityData, bool>> predicate);
    }
}