using GalaSoft.MvvmLight;
using OneDo.Application.Common;

namespace OneDo.ViewModel
{
    public abstract class ItemViewModel<TEntity> : ObservableObject 
        where TEntity : IEntityModel
    {
        public TEntity Entity { get; }

        protected ItemViewModel(TEntity entity)
        {
            Entity = entity;
        }

        public void Refresh()
        {
            RaisePropertyChanged("");
        }
    }
}
