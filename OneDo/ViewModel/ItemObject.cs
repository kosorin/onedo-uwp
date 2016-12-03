using GalaSoft.MvvmLight;
using OneDo.Model;

namespace OneDo.ViewModel
{
    public abstract class ItemObject<TEntity> : ObservableObject where TEntity : IEntity
    {
        public TEntity Entity { get; }

        protected ItemObject(TEntity entity)
        {
            Entity = entity;
        }

        public void Refresh()
        {
            RaisePropertyChanged("");
        }
    }
}
