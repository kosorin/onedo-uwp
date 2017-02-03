using OneDo.Infrastructure.Data.Entities;
using OneDo.Domain.Common;
using SQLite.Net.Async;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OneDo.Infrastructure.Data.Repositories
{
    public interface IRepository<TEntityData> : IQueryRepository<TEntityData>, ICommandRepository<TEntityData> where TEntityData : class, IEntityData
    {

    }
}