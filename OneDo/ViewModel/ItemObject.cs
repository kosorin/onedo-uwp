using GalaSoft.MvvmLight;
using OneDo.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
