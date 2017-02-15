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
    public class DailyRecurrenceTests
    {
        private readonly DateTime from = new DateTime(2017, 2, 15, 16, 30, 0);

        [TestMethod]
        public void GetOccurrences_Every()
        {
            var recurrence = new DailyRecurrence(1, null);
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
        public void GetOccurrences_EveryThird()
        {
            var recurrence = new DailyRecurrence(3, null);
            var occurrences = recurrence.GetOccurrences(from).Take(5).ToList();

            CollectionAssert.AreEqual(new[]
            {
                from.AddDays(0),
                from.AddDays(3),
                from.AddDays(6),
                from.AddDays(9),
                from.AddDays(12),
            }, occurrences);
        }

        [TestMethod]
        public void GetOccurrences_Every_Until()
        {
            var recurrence = new DailyRecurrence(1, new DateTime(2017, 2, 19));
            var occurrences = recurrence.GetOccurrences(from).ToList();

            CollectionAssert.AreEqual(new[]
            {
                from.AddDays(0),
                from.AddDays(1),
                from.AddDays(2),
                from.AddDays(3),
                from.AddDays(4),
            }, occurrences);
        }

        [TestMethod]
        public void GetOccurrences_EveryThird_Until()
        {
            var recurrence = new DailyRecurrence(3, new DateTime(2017, 2, 26));
            var occurrences = recurrence.GetOccurrences(from).ToList();

            CollectionAssert.AreEqual(new[]
            {
                from.AddDays(0),
                from.AddDays(3),
                from.AddDays(6),
                from.AddDays(9),
            }, occurrences);
        }
    }
}
