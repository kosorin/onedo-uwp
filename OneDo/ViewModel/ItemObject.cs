using GalaSoft.MvvmLight;
using OneDo.Application.Common;

namespace OneDo.ViewModel
{
    public abstract class ItemObject<TEntityModel> : ObservableObject where TEntityModel : IEntityModel
    {
        public TEntityModel EntityModel { get; }

        protected ItemObject(TEntityModel entityModel)
        {
            EntityModel = entityModel;
        }

        public void Refresh()
        {
            RaisePropertyChanged("");
        }
    }
}
