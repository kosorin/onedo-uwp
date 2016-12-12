﻿using OneDo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OneDo.Data.Repositories
{
    public interface IRepository
    {
    }

    public interface IRepository<TEntity> : IRepository where TEntity : class, IEntity
    {
        //event EventHandler<EntityEventArgs<TEntity>> Added;

        //event EventHandler<EntityEventArgs<TEntity>> Updated;

        //event EventHandler<EntityEventArgs<TEntity>> Deleted;

        //event EventHandler<EntityListEventArgs<TEntity>> BulkDeleted;


        bool IsTransient(TEntity entity);


        Task AddOrUpdate(TEntity entity);

        Task Add(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(TEntity entity);

        Task DeleteAll();

        Task DeleteAll(Expression<Func<TEntity, bool>> predicate);


        Task<IList<TEntity>> GetAll();

        Task<IList<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> Get(Guid id);

        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        Task<bool> Any();

        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
    }
}