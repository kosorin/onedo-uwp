using OneDo.Common.UI;
using OneDo.Model.Business;
using OneDo.Model.Data;
using OneDo.ViewModel.Args;
using OneDo.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace OneDo.ViewModel
{
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

        public string TimeText => DateTimeBusiness.TimeToString(Time) ?? PlaceholderText;

        public event TypedEventHandler<TimePickerViewModel, TimePickerEventArgs> TimeChanged;

        public IExtendedCommand ClearCommand { get; }

        public IExtendedCommand InOneMinuteCommand { get; }

        public IExtendedCommand InFiveMinuteCommand { get; }

        public DateTimeBusiness DateTimeBusiness { get; }

        public TimePickerViewModel(DataService dataService, string placeholderText)
        {
            DateTimeBusiness = new DateTimeBusiness(dataService);
            PlaceholderText = placeholderText;

            ClearCommand = new RelayCommand(() => Time = null);
            InOneMinuteCommand = new RelayCommand(() => Time = DateTimeBusiness.CurrentTime().Add(TimeSpan.FromMinutes(1)));
            InFiveMinuteCommand = new RelayCommand(() => Time = DateTimeBusiness.CurrentTime().Add(TimeSpan.FromMinutes(5)));
        }

        private void OnTimeChanged()
        {
            RaisePropertyChanged(nameof(TimeText));
            TimeChanged?.Invoke(this, new TimePickerEventArgs(Time));
        }
    }
}