using GalaSoft.MvvmLight;

namespace OneDo.Common.Mvvm
{
    public abstract class ExtendedViewModel : ViewModelBase
    {
#if false
        protected ExtendedViewModel()
        {
            Logger.Current.Trace($"{GetType().Name}.ctor");
        }
#endif
    }
}
