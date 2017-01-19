using OneDo.Common.Extensions;
using OneDo.Common.Mvvm;
using OneDo.ViewModel.Args;
using System;
using Windows.Foundation;

namespace OneDo.ViewModel
{
#warning Přesunout do View, aby control nebyl závislý na VM
    public class DatePickerViewModel : ModalViewModel
    {
        private DateTime? date;
        public DateTime? Date
        {
            get { return date; }
            set
            {
                Set(ref date, value?.Date);
                Refresh();
            }
        }

        private string placeholderText;
        public string PlaceholderText
        {
            get { return placeholderText; }
            set { Set(ref placeholderText, value); }
        }

        public string DateText => Date?.ToLongDateString() ?? PlaceholderText;

        public event TypedEventHandler<DatePickerViewModel, DatePickerEventArgs> DateChanged;

        public IExtendedCommand ClearCommand { get; }

        public IExtendedCommand TodayCommand { get; }

        public IExtendedCommand TomorrowCommand { get; }

        public IExtendedCommand ThisWeekCommand { get; }

        public IExtendedCommand NextWeekCommand { get; }

        public DatePickerViewModel(string placeholderText)
        {
            PlaceholderText = placeholderText;

            ClearCommand = new RelayCommand(() => Date = null);
            TodayCommand = new RelayCommand(() => Date = DateTime.Today);
            TomorrowCommand = new RelayCommand(() => Date = DateTime.Today.Tomorrow());
            ThisWeekCommand = new RelayCommand(() => Date = DateTime.Today.LastDayOfWeek(DayOfWeek.Monday));
            NextWeekCommand = new RelayCommand(() => Date = DateTime.Today.LastDayOfWeek(DayOfWeek.Monday).AddWeeks(1));
        }

        public void Refresh()
        {
            RaisePropertyChanged(nameof(DateText));
            DateChanged?.Invoke(this, new DatePickerEventArgs(Date));
        }
    }
}
