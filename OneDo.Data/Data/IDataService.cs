using OneDo.Infrastructure.Data.Entities;
using OneDo.Infrastructure.Data.Repositories;
using System;

namespace OneDo.Infrastructure.Data
{
    public interface IDataService : IDisposable
    {
        IRepository<TEntityData> GetRepository<TEntityData>() where TEntityData : class, IEntityData;

        IQueryRepository<TEntityData> GetQueryRepository<TEntityData>() where TEntityData : class, IEntityData;

        ICommandRepository<TEntityData> GetCommandRepository<TEntityData>() where TEntityData : class, IEntityData;
    }
}