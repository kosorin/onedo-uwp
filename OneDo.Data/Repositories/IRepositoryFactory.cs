using OneDo.Infrastructure.Entities;

namespace OneDo.Infrastructure.Repositories
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;

        IQueryRepository<TEntity> GetQueryRepository<TEntity>() where TEntity : class, IEntity;

        ICommandRepository<TEntity> GetCommandRepository<TEntity>() where TEntity : class, IEntity;
    }
}