using OneDo.Model.Data;
using System;

namespace OneDo.Model.Business
{
    public class EntityEventArgs<TEntity> : EventArgs where TEntity : IEntity
    {
        public TEntity Entity { get; }

        public EntityEventArgs(TEntity entity)
        {
            Entity = entity;
        }
    }
}