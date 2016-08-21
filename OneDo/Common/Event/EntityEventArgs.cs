using OneDo.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Common.Event
{
    public class EntityEventArgs<TEntity> : EventArgs where TEntity : IEntity
    {
        public TEntity Entity { get; set; }

        public EntityEventArgs(TEntity entity)
        {
            Entity = entity;
        }
    }
}
