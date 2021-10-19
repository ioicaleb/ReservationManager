using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTests
{
    [TestClass]
    public class DepartmentDAOTests : ProjectTestsBase
    {
        [TestMethod]
        public void GettingACollectionOfDepartmentsResultsInCorrectNumber()
        {
            // Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);

            // Act
            IEnumerable<Department> results = dao.GetDepartments();

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public void CheckAddingNewDepartment()
        {

        }
    }
}
