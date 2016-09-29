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
        public EntityBusiness(IDataService dataService) : base(dataService)
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
                if (IsNew(entity))
                {
                DataService.
                    dc.Set<TEntity>().Add(entity);
                }
                else
                {
                    dc.Set<TEntity>().Update(entity);
                }
        }

        public async Task Delete(TEntity entity)
        {
            //TODO:using (var dc = new DataContext())
            //{
            //    dc.Set<TEntity>().Remove(entity);
            //    await dc.SaveChangesAsync();
            //}
        }

        public abstract TEntity Clone(TEntity entity);
    }
}
