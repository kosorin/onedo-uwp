using OneDo.Data.Entities;

namespace OneDo.Data.Repositories
{
    public interface IRepositoryFactory
    {
        IQueryRepository<TEntity> GetQueryRepository<TEntity>() where TEntity : class, IEntity;

        ICommandRepository<TEntity> GetCommandRepository<TEntity>() where TEntity : class, IEntity;
    }
}