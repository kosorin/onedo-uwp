using OneDo.Infrastructure.Data.Entities;
using OneDo.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OneDo.Infrastructure.Data.Repositories
{
    public interface ICommandRepository<TEntityData> where TEntityData : class, IEntityData
    {
        Task Add(TEntityData entity);

        Task Update(TEntityData entity);

        Task Delete(Guid id);

        Task DeleteAll();

        Task DeleteAll(Expression<Func<TEntityData, bool>> predicate);
    }
}