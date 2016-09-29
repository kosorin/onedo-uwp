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
        private readonly Repository<TEntity> repository;

        public EntityBusiness(DataService dataService) : base(dataService)
        {
            repository = dataService.GetRepository<TEntity>();
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
                await repository.Add(entity);
            }
            else
            {
                await repository.Update(entity);
            }
        }

        public async Task Delete(TEntity entity)
        {
            await repository.Delete(entity);
        }

        public abstract TEntity Clone(TEntity entity);
    }
}
