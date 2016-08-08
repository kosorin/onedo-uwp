using GalaSoft.MvvmLight;
using OneDo.Model.Data;
using OneDo.Services.Context;
using OneDo.Services.NavigationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModels
{
    public abstract class FlyoutViewModel : ExtendedViewModel
    {
        public INavigationService NavigationService { get; }

        public IDataProvider DataProvider { get; }

        public IContext Context { get; }

        protected FlyoutViewModel(INavigationService navigationService, IDataProvider dataProvider, IContext context)
        {
            NavigationService = navigationService;
            DataProvider = dataProvider;
            Context = context;
        }
    }
}
