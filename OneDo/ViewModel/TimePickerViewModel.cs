using OneDo.Common.Extensions;
using OneDo.Common.Mvvm;
using OneDo.ViewModel.Args;
using System;
using Windows.Foundation;

namespace OneDo.ViewModel
{
#warning Přesunout do View, aby control nebyl závislý na VM
    public class TimePickerViewModel : ModalViewModel
    {
        private TimeSpan? time;
        public TimeSpan? Time
        {
            get { return time; }
            set
            {
                Set(ref time, value);
                OnTimeChanged();
            }
        }

        private string placeholderText;
        public string PlaceholderText
        {
            get { return placeholderText; }
            set { Set(ref placeholderText, value); }
        }

        public string TimeText => Time?.ToTimeString() ?? PlaceholderText;

        public event TypedEventHandler<TimePickerViewModel, TimePickerEventArgs> TimeChanged;

        public IExtendedCommand ClearCommand { get; }

        public IExtendedCommand InOneMinuteCommand { get; }

        public IExtendedCommand InFiveMinuteCommand { get; }

        public TimePickerViewModel(string placeholderText)
        {
            PlaceholderText = placeholderText;

            ClearCommand = new RelayCommand(() => Time = null);
            InOneMinuteCommand = new RelayCommand(() => Time = DateTime.Now.ToTime() + TimeSpan.FromMinutes(1));
            InFiveMinuteCommand = new RelayCommand(() => Time = DateTime.Now.ToTime() + TimeSpan.FromMinutes(5));
        }

        private void OnTimeChanged()
        {
            RaisePropertyChanged(nameof(TimeText));
            TimeChanged?.Invoke(this, new TimePickerEventArgs(Time));
        }
    }
}