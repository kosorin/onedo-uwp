using OneDo.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel.Args
{
    public class EntityEventArgs<TEntity> : EventArgs 
        where TEntity : IEntityModel
    {
        public TEntity Entity { get; }

        public EntityEventArgs(TEntity entity)
        {
            Entity = entity;
        }
    }
}
