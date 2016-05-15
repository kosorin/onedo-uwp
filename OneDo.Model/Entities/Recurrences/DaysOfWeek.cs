using System;

namespace OneDo.Model.Entities.Recurrences
{
    [Flags]
    public enum DaysOfWeek
    {
        None = 0,

        Monday = 1,
        Tuesday = 2,
        Wednesday = 4,
        Thursday = 8,
        Friday = 16,
        Saturday = 32,
        Sunday = 64,

        Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday,
        Weekends = Saturday | Sunday,

        EveryDay = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday,
    }
}
