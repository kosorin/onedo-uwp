using GalaSoft.MvvmLight;
using OneDo.Model.Data;
using OneDo.Services.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModels
{
    public class EditorViewModel : ExtendedViewModelBase
    {
        public IDataProvider DataProvider { get; }

        public IContext Context { get; }

        protected EditorViewModel(IDataProvider dataProvider, IContext context)
        {
            DataProvider = dataProvider;
            Context = context;
        }
    }
}
