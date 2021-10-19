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
    public class EmployeeDAOTests : ProjectTestsBase
    {
        [TestMethod]
        public void GetAllReturnstheCorrectNumberofEmployees()
        {
            //Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(ConnectionString);

            //Act
            IEnumerable<Employee> result = dao.GetAllEmployees();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        [DataRow("Caleb", "Gaffney", 1)]
        [DataRow("Kevin", "Teos", 0)]
        public void SearchReturnsCorrectList(string firstname, string lastname, int expected)
        {
            //Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(ConnectionString);

            //Act
            ICollection<Employee> result = dao.Search(firstname, lastname);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.Count);
        }

        [TestMethod]
        public void GetEmployeesWithoutProjectsReturnsCorrectList()
        {
            //Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(ConnectionString);
            ProjectSqlDAO project = new ProjectSqlDAO(ConnectionString);

            //Act
            project.AssignEmployeeToProject(1, 1);
            ICollection<Employee> result = dao.GetEmployeesWithoutProjects();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }
    }
}
