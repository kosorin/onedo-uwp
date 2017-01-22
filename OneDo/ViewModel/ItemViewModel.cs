using GalaSoft.MvvmLight;
using OneDo.Application.Common;
using System;

namespace OneDo.ViewModel
{
    public abstract class ItemViewModel<TEntity> : ObservableObject
        where TEntity : IEntityModel
    {
        public Guid Id { get; }

        protected ItemViewModel(Guid id)
        {
            Id = id;
        }
    }
}
