using GalaSoft.MvvmLight;

namespace OneDo.Mvvm
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
