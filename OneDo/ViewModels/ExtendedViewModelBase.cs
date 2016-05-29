using GalaSoft.MvvmLight;
using OneDo.Common.Logging;

namespace OneDo.ViewModels
{
    public abstract class ExtendedViewModelBase : ViewModelBase
    {
        protected ExtendedViewModelBase()
        {
            Logger.Current.Trace($"{GetType().Name}.ctor");
        }
    }
}
