using OneDo.ViewModel.Parameters;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace OneDo.View
{
    public class ModalView : ExtendedUserControl
    {
        public IParameters Parameters { get; set; }

        public virtual ModalContainer SubContainer => null;

        [Obsolete("Po smazani parametrů zrusit obsolete")]
        public ModalView()
        {
        }

        public ModalView(IParameters parameters)
        {
            Parameters = parameters;

            Loaded += OnModalLoaded;
        }

        private void OnModalLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnModalLoaded;
            OnFirstLoad();
        }

        protected virtual Task OnFirstLoad()
        {
            return Task.CompletedTask;
        }
    }
}