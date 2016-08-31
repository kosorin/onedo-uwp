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
            return entity.Id == default(int);
        }

        public virtual TEntity Default()
        {
            return new TEntity();
        }

        public async Task Save(TEntity entity)
        {
            using (var dc = new DataContext())
            {
                if (IsNew(entity))
                {
                    dc.Set<TEntity>().Add(entity);
                }
                else
                {
                    dc.Set<TEntity>().Update(entity);
                }
                await dc.SaveChangesAsync();
            }
        }

        public async Task Delete(TEntity entity)
        {
            using (var dc = new DataContext())
            {
                dc.Set<TEntity>().Remove(entity);
                await dc.SaveChangesAsync();
            }
        }

        public abstract TEntity Clone(TEntity entity);
    }
}
