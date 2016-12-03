using GalaSoft.MvvmLight;
using OneDo.Common.Logging;
using System.Threading.Tasks;

namespace OneDo.ViewModel
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
