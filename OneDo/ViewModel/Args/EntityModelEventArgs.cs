using OneDo.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel.Args
{
    public class EntityModelEventArgs<TEntityModel> : EventArgs where TEntityModel : IEntityModel
    {
        public TEntityModel EntityModel { get; }

        public EntityModelEventArgs(TEntityModel entityModel)
        {
            EntityModel = entityModel;
        }
    }
}
