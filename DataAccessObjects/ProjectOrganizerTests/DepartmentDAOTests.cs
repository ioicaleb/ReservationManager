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
        public void CheckCreatingDepartmentInsertsIntoTable()
        {
            // Arrange
            // adding a new value to the name of the inserting row
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);
            Department dept = new Department
            {
                Name = "Test"
            };

            // Act
            int id = dao.CreateDepartment(dept);

            // Assert
            Assert.IsTrue(id > 1, "Added department looks to be invalid");
            Assert.AreEqual(2, GetRowCount("department"));
        }

        [TestMethod]
        public void CheckUpdateDepartmentFailsIfDepartmentExist()
        {
            // Arrange
            // Adding a new value to the name of the inserting row
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);
            Department dept = new Department
            {
                Name = "Department of Doing the Code"
            };

            // Act
            int id = dao.CreateDepartment(dept);

            // Assert
            Assert.IsTrue(id < 0);
        }

        [TestMethod]
        public void UpdatingACurrentDepartmentShouldNotAddRow()
        {
            // Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);
            Department dept = new Department
            {
                Name = "New Test"
            };

            // Act
            bool result = dao.UpdateDepartment(dept);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, GetRowCount("department"));
        }
    }
}
