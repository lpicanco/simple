﻿using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using SharpTestsEx;
using Simple.Tests.Resources;
using Simple.Data;

namespace Simple.Tests.Data
{
    [TestFixture]
    public class NHLinqFixture : BaseDataFixture
    {
        [Test]
        public void GroupTerritoriesByEmployee()
        {
            var mapping = Session.Query<EmployeeTerritory>();

            var q = mapping.GroupBy(x => x.Employee.Id)
                .Select(x => new { x.Key, Count = x.Count() });

            var list = q.ToDictionary(x => x.Key);

            list.Count.Should().Be(9);

            list[1].Count.Should().Be(2);
            list[2].Count.Should().Be(7);
            list[3].Count.Should().Be(4);
            list[4].Count.Should().Be(3);
            list[5].Count.Should().Be(7);
            list[6].Count.Should().Be(5);
            list[7].Count.Should().Be(10);
            list[8].Count.Should().Be(4);
            list[9].Count.Should().Be(7);
        }

        [Test]
        public void SafeSumEmptyTableWithDefaultValueAndFilter()
        {
            Employee emp = null;
            var mapping = Session.Query<EmployeeTerritory>().Where(x => x.Employee == emp);
            mapping.SafeAggregate(q => q.Max(x => x.Employee.Id), 42).Should().Be(42);
            
            emp = EmployeeTerritory.Find(x => true).Employee;
            mapping.SafeAggregate(q => q.Max(x => x.Employee.Id), 42).Should().Be(emp.Id);
        }


        [Test]
        public void SafeSumEmptyTableWithDefaultValue()
        {
            EmployeeTerritory.Delete(x => true);

            var mapping = Session.Query<EmployeeTerritory>();

            mapping.SafeAggregate(q => q.Sum(x => x.Employee.Id), 10).Should().Be(10);
        }

        [Test]
        public void SafeSumEmptyTableWithoutDefaultValue()
        {
            EmployeeTerritory.Delete(x => true);

            var mapping = Session.Query<EmployeeTerritory>();

            mapping.SafeAggregate(q => q.Sum(x => x.Employee.Id)).Should().Be(0);
        }

    }
}
