using OneDo.Model.Data;
using System;
using System.Collections.Generic;

namespace OneDo.Model.Args
{
    public class EntityListEventArgs<TEntity> : EventArgs where TEntity : IEntity
    {
        public List<TEntity> Entities { get; }

        public EntityListEventArgs(List<TEntity> entities)
        {
            Entities = entities;
        }
    }
}