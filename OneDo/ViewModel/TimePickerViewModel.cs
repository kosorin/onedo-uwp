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

        public string TimeText => dateTimeBusiness.TimeToString(Time) ?? "Set Reminder";

        public event TypedEventHandler<TimePickerViewModel, TimePickerEventArgs> TimeChanged;

        public IExtendedCommand ClearCommand { get; }


        private readonly DateTimeBusiness dateTimeBusiness;

        public TimePickerViewModel(DataService dataService)
        {
            dateTimeBusiness = new DateTimeBusiness(dataService);

            ClearCommand = new RelayCommand(() => Time = null);
        }

        private void OnTimeChanged()
        {
            RaisePropertyChanged(nameof(TimeText));
            TimeChanged?.Invoke(this, new TimePickerEventArgs(Time));
        }
    }
}