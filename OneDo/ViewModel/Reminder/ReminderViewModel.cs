﻿using OneDo.Application.Models;
using OneDo.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel.Reminder
{
    public class ReminderViewModel : ExtendedViewModel
    {
        private ReminderModel reminder;
        public ReminderModel Reminder
        {
            get { return reminder; }
            private set
            {
                if (Set(ref reminder, value))
                {
                }
            }
        }
    }
}
