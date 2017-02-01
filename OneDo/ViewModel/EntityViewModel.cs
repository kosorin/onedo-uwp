using GalaSoft.MvvmLight;
using OneDo.Application.Common;
using System;

namespace OneDo.ViewModel
{
    public abstract class EntityViewModel<TEntity> : ObservableObject
        where TEntity : IEntityModel
    {
        public Guid Id { get; }

        protected EntityViewModel(Guid id)
        {
            Id = id;
        }

        public abstract void Update(TEntity entity);
    }
}
