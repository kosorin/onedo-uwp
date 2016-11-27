using OneDo.Model.Data;
using System;
using System.Collections.Generic;

namespace OneDo.Model.Args
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