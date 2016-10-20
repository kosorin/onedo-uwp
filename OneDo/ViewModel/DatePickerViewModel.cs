using OneDo.Common.UI;
using OneDo.Model.Business;
using OneDo.Model.Business.Validation;
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
    public class DatePickerViewModel : ExtendedViewModel
    {
        private DateTime? date;
        public DateTime? Date
        {
            get { return date; }
            set
            {
                if (Set(ref date, value?.Date))
                {
                    OnDateChanged();
                }
            }
        }

        public string DateText => dateTimeBusiness.DateToString(Date) ?? "Set Date & Reminder";

        public event TypedEventHandler<DatePickerViewModel, DatePickerEventArgs> DateChanged;

        public IExtendedCommand TodayCommand { get; }

        public IExtendedCommand TomorrowCommand { get; }

        public IExtendedCommand ThisWeekCommand { get; }

        public IExtendedCommand NextWeekCommand { get; }

        public DataService DataService { get; }

        private readonly DateTimeBusiness dateTimeBusiness;

        public DatePickerViewModel(DataService dataService)
        {
            dateTimeBusiness = new DateTimeBusiness(DataService);

            TodayCommand = new RelayCommand(() => Date = dateTimeBusiness.Today());
            TomorrowCommand = new RelayCommand(() => Date = dateTimeBusiness.Tomorrow());
            ThisWeekCommand = new RelayCommand(() => Date = dateTimeBusiness.ThisWeek());
            NextWeekCommand = new RelayCommand(() => Date = dateTimeBusiness.NextWeek());
        }

        private void OnDateChanged()
        {
            RaisePropertyChanged(nameof(DateText));
            DateChanged?.Invoke(this, new DatePickerEventArgs(Date));
        }
    }
}
