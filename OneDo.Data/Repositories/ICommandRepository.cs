using OneDo.Infrastructure.Entities;
using OneDo.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OneDo.Infrastructure.Repositories
{
    public interface ICommandRepository<TEntity> where TEntity : class, IEntity
    {
        Task Add(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(Guid id);

        Task DeleteAll();

        Task DeleteAll(Expression<Func<TEntity, bool>> predicate);
    }
}