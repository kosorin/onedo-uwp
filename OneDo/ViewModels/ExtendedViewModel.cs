﻿using GalaSoft.MvvmLight;
using OneDo.Common.Logging;

namespace OneDo.ViewModels
{
    public abstract class ExtendedViewModel : ViewModelBase
    {
        protected ExtendedViewModel()
        {
            Logger.Current.Trace($"{GetType().Name}.ctor");
        }
    }
}