using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneDo.Common;
using OneDo.Domain.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Tests.Model.ValueObjects
{
    [TestClass]
    public class WeeklyRecurrenceTests
    {
        private readonly DateTime from = new DateTime(2017, 2, 15, 16, 30, 0);

        [TestMethod]
        public void GetOccurrences_EveryDay()
        {
            var recurrence = new WeeklyRecurrence(DaysOfWeek.EveryDay, 1, null);
            var occurrences = recurrence.GetOccurrences(from).Take(9).ToList();

            CollectionAssert.AreEqual(new[]
            {
                from.AddDays(0),
                from.AddDays(1),
                from.AddDays(2),
                from.AddDays(3),
                from.AddDays(4),
                from.AddDays(5),
                from.AddDays(6),
                from.AddDays(7),
                from.AddDays(8),
            }, occurrences);
        }

        [TestMethod]
        public void GetOccurrences_Weekends()
        {
            var recurrence = new WeeklyRecurrence(DaysOfWeek.Weekends, 1, null);
            var occurrences = recurrence.GetOccurrences(from).Take(5).ToList();

            CollectionAssert.AreEqual(new[]
            {
                from.AddDays(3),
                from.AddDays(4),
                from.AddDays(10),
                from.AddDays(11),
                from.AddDays(17),
            }, occurrences);
        }

        [TestMethod]
        public void GetOccurrences_EveryDay_EverySecond()
        {
            var recurrence = new WeeklyRecurrence(DaysOfWeek.EveryDay, 2, null);
            var occurrences = recurrence.GetOccurrences(from).Take(9).ToList();

            CollectionAssert.AreEqual(new[]
            {
                from.AddDays(0),
                from.AddDays(1),
                from.AddDays(2),
                from.AddDays(3),
                from.AddDays(4),
                from.AddDays(5),
                from.AddDays(6),
                from.AddDays(14),
                from.AddDays(15),
            }, occurrences);
        }

        [TestMethod]
        public void GetOccurrences_Weekends_EverySecond()
        {
            var recurrence = new WeeklyRecurrence(DaysOfWeek.Weekends, 2, null);
            var occurrences = recurrence.GetOccurrences(from).Take(5).ToList();

            CollectionAssert.AreEqual(new[]
            {
                from.AddDays(3),
                from.AddDays(4),
                from.AddDays(17),
                from.AddDays(18),
                from.AddDays(31),
            }, occurrences);
        }

        [TestMethod]
        public void GetOccurrences_EveryDay_Until()
        {
            var recurrence = new WeeklyRecurrence(DaysOfWeek.EveryDay, 1, new DateTime(2017, 2, 25));
            var occurrences = recurrence.GetOccurrences(from).ToList();

            CollectionAssert.AreEqual(new[]
            {
                from.AddDays(0),
                from.AddDays(1),
                from.AddDays(2),
                from.AddDays(3),
                from.AddDays(4),
                from.AddDays(5),
                from.AddDays(6),
                from.AddDays(7),
                from.AddDays(8),
                from.AddDays(9),
                from.AddDays(10),
            }, occurrences);
        }

        [TestMethod]
        public void GetOccurrences_Weekends_Until()
        {
            var recurrence = new WeeklyRecurrence(DaysOfWeek.Weekends, 1, new DateTime(2017, 2, 25));
            var occurrences = recurrence.GetOccurrences(from).ToList();

            CollectionAssert.AreEqual(new[]
            {
                from.AddDays(3),
                from.AddDays(4),
                from.AddDays(10),
            }, occurrences);
        }

        [TestMethod]
        public void GetOccurrences_NoneDaysOfWeek()
        {
            var recurrence = new WeeklyRecurrence(DaysOfWeek.None, 2, null);
            var occurrences = recurrence.GetOccurrences(from).Take(4).ToList();

            CollectionAssert.AreEqual(new[]
            {
                from.AddDays(0),
                from.AddDays(14),
                from.AddDays(28),
                from.AddDays(42),
            }, occurrences);
        }
    }
}
