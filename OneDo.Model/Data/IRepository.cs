using OneDo.Model.Args;
using OneDo.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OneDo.Model.Data
{
    public interface IRepository
    {
    }

    public interface IRepository<TEntity> : IRepository where TEntity : class, IEntity
    {
        event EventHandler<EntityEventArgs<TEntity>> Added;

        event EventHandler<EntityEventArgs<TEntity>> Updated;

        event EventHandler<EntityEventArgs<TEntity>> Deleted;

        event EventHandler<EntityListEventArgs<TEntity>> BulkDeleted;


        bool IsNew(TEntity entity);

        Task AddOrUpdate(TEntity entity);

        Task Add(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(TEntity entity);

        Task DeleteAll();

        Task DeleteAll(Expression<Func<TEntity, bool>> predicate);


        Task<List<TEntity>> GetAll();

        Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> Get(int id);

        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        Task<bool> Any();

        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
    }
}