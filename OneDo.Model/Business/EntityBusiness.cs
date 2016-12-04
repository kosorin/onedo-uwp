using OneDo.Model.Data;

namespace OneDo.Model.Business
{
    public abstract class EntityBusiness<TEntity> where TEntity : class, IEntity, new()
    {
        public DataService DataService { get; }

        public EntityBusiness(DataService dataService)
        {
            DataService = dataService;
        }

        public virtual TEntity CreateDefault()
        {
            return new TEntity();
        }

        public string GetToastGroup(TEntity entity)
        {
            return entity.Id.ToString("N").Substring(0, 15);
        }
    }
}
