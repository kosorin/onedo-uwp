using OneDo.Common.UI;
using OneDo.Model.Business;
using OneDo.Model.Data;
using OneDo.Services.ModalService;
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

        public string DateText => dateTimeBusiness.DateToLongString(Date) ?? PlaceholderText;

        public event TypedEventHandler<DatePickerViewModel, DatePickerEventArgs> DateChanged;

        public IExtendedCommand ClearCommand { get; }

        public IExtendedCommand TodayCommand { get; }

        public IExtendedCommand TomorrowCommand { get; }

        public IExtendedCommand ThisWeekCommand { get; }

        public IExtendedCommand NextWeekCommand { get; }

        private readonly DateTimeBusiness dateTimeBusiness;

        public DatePickerViewModel(DataService dataService, string placeholderText)
        {
            dateTimeBusiness = new DateTimeBusiness(dataService);
            PlaceholderText = placeholderText;

            ClearCommand = new RelayCommand(() => Date = null);
            TodayCommand = new RelayCommand(() => Date = dateTimeBusiness.Today());
            TomorrowCommand = new RelayCommand(() => Date = dateTimeBusiness.Tomorrow());
            ThisWeekCommand = new RelayCommand(() => Date = dateTimeBusiness.ThisWeek());
            NextWeekCommand = new RelayCommand(() => Date = dateTimeBusiness.NextWeek());
        }

        public void Refresh()
        {
            RaisePropertyChanged(nameof(DateText));
            DateChanged?.Invoke(this, new DatePickerEventArgs(Date));
        }
    }
}
