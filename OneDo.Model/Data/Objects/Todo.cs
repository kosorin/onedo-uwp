﻿using OneDo.Model.Data.Objects.Recurrences;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace OneDo.Model.Data.Objects
{
    [KnownType(typeof(DailyRecurrence))]
    [KnownType(typeof(WeeklyRecurrence))]
    [KnownType(typeof(MonthlyRecurrence))]
    [DebuggerDisplay("{Id}: {Title}")]
    public class Todo
    {
        public Guid Id { get; set; }

        public Guid? Parent { get; set; }


        public string Title { get; set; }

        public string Note { get; set; }


        public DateTime? Date { get; set; }

        public TimeSpan? Reminder { get; set; }

        public RecurrenceBase Recurrence { get; set; }


        public bool Flag { get; set; }

        public List<Guid> Tags { get; set; }


        public DateTime? Completed { get; set; }

        public DateTime? Deleted { get; set; }
    }
}
