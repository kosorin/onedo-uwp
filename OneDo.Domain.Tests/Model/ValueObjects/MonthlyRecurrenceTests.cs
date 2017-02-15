using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneDo.Domain.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Tests.Model.ValueObjects
{
    [TestClass]
    public class MonthlyRecurrenceTests
    {
        private readonly TimeSpan time = new TimeSpan(16, 30, 0);

        [TestMethod]
        public void GetOccurrences_Every()
        {
            var from = new DateTime(2017, 2, 15) + time;
            var recurrence = new MonthlyRecurrence(1, null);
            var occurrences = recurrence.GetOccurrences(from).Take(4).ToList();

            CollectionAssert.AreEqual(new[]
            {
                new DateTime(2017, 2, 15) + time,
                new DateTime(2017, 3, 15) + time,
                new DateTime(2017, 4, 15) + time,
                new DateTime(2017, 5, 15) + time,
            }, occurrences);
        }

        [TestMethod]
        public void GetOccurrences_EveryThird()
        {
            var from = new DateTime(2017, 2, 15) + time;
            var recurrence = new MonthlyRecurrence(3, null);
            var occurrences = recurrence.GetOccurrences(from).Take(4).ToList();

            CollectionAssert.AreEqual(new[]
            {
                new DateTime(2017, 2, 15) + time,
                new DateTime(2017, 5, 15) + time,
                new DateTime(2017, 8, 15) + time,
                new DateTime(2017, 11, 15) + time,
            }, occurrences);
        }

        [TestMethod]
        public void GetOccurrences_Every_Until()
        {
            var from = new DateTime(2017, 2, 15) + time;
            var recurrence = new MonthlyRecurrence(1, new DateTime(2017, 4, 19));
            var occurrences = recurrence.GetOccurrences(from).ToList();

            CollectionAssert.AreEqual(new[]
            {
                new DateTime(2017, 2, 15) + time,
                new DateTime(2017, 3, 15) + time,
                new DateTime(2017, 4, 15) + time,
            }, occurrences);
        }

        [TestMethod]
        public void GetOccurrences_EveryThird_Until()
        {
            var from = new DateTime(2017, 2, 15) + time;
            var recurrence = new MonthlyRecurrence(3, new DateTime(2017, 9, 15));
            var occurrences = recurrence.GetOccurrences(from).ToList();

            CollectionAssert.AreEqual(new[]
            {
                new DateTime(2017, 2, 15) + time,
                new DateTime(2017, 5, 15) + time,
                new DateTime(2017, 8, 15) + time,
            }, occurrences);
        }


        [TestMethod]
        public void GetOccurrences_EveryEndOfMonth()
        {
            var from = new DateTime(2017, 1, 31) + time;
            var recurrence = new MonthlyRecurrence(1, null);
            var occurrences = recurrence.GetOccurrences(from).Take(4).ToList();

            CollectionAssert.AreEqual(new[]
            {
                new DateTime(2017, 1, 31) + time,
                new DateTime(2017, 2, 28) + time,
                new DateTime(2017, 3, 31) + time,
                new DateTime(2017, 4, 30) + time,
            }, occurrences);
        }

        [TestMethod]
        public void GetOccurrences_EveryThirdEndOfMonth()
        {
            var from = new DateTime(2017, 1, 31) + time;
            var recurrence = new MonthlyRecurrence(3, null);
            var occurrences = recurrence.GetOccurrences(from).Take(4).ToList();

            CollectionAssert.AreEqual(new[]
            {
                new DateTime(2017, 1, 31) + time,
                new DateTime(2017, 4, 30) + time,
                new DateTime(2017, 7, 31) + time,
                new DateTime(2017, 10, 31) + time,
            }, occurrences);
        }

        [TestMethod]
        public void GetOccurrences_EveryEndOfMonth_Until()
        {
            var from = new DateTime(2017, 1, 31) + time;
            var recurrence = new MonthlyRecurrence(1, new DateTime(2017, 4, 1));
            var occurrences = recurrence.GetOccurrences(from).ToList();

            CollectionAssert.AreEqual(new[]
            {
                new DateTime(2017, 1, 31) + time,
                new DateTime(2017, 2, 28) + time,
                new DateTime(2017, 3, 31) + time,
            }, occurrences);
        }

        [TestMethod]
        public void GetOccurrences_EveryThirdEndOfMonth_Until()
        {
            var from = new DateTime(2017, 1, 31) + time;
            var recurrence = new MonthlyRecurrence(3, new DateTime(2017, 10, 30));
            var occurrences = recurrence.GetOccurrences(from).ToList();

            CollectionAssert.AreEqual(new[]
            {
                new DateTime(2017, 1, 31) + time,
                new DateTime(2017, 4, 30) + time,
                new DateTime(2017, 7, 31) + time,
            }, occurrences);
        }
    }
}