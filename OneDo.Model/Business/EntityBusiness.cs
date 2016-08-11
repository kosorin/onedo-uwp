using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Model.Data;
using OneDo.Model.Data.Entities;

namespace OneDo.Model.Business
{
    public abstract class EntityBusiness<TEntity> : BusinessBase where TEntity : class, IEntity, new()
    {
        public EntityBusiness(ISettingsProvider settingsProvider) : base(settingsProvider)
        {

        }

        public bool IsNew(TEntity entity)
        {
            return entity.Id == default(Guid);
        }

        public virtual TEntity Default()
        {
            return new TEntity();
        }

        public abstract TEntity Clone(TEntity entity);
    }
}
