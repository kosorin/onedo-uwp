using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Model.Data;
using OneDo.Model.Data.Entities;
using System.Linq.Expressions;

namespace OneDo.Model.Business
{
    public abstract class EntityBusiness<TEntity> : DataBusinessBase where TEntity : class, IEntity, new()
    {
        public EntityBusiness(DataService dataService) : base(dataService)
        {
        }

        public virtual TEntity CreateDefault()
        {
            return new TEntity();
        }
    }
}
