using System;

namespace OneDo.Model.Entities.Args
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