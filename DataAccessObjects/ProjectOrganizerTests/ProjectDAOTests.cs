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
    public class ProjectDAOTests : ProjectTestsBase
    {
        [TestMethod]
        public void GettingProjectGetsCorrectNumber()
        {
            //Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);

            //Act
            IEnumerable<Project> result = dao.GetAllProjects();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void AssignEmployeeAddsEmployeeToProject()
        {
            //Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);

            //Act
            bool result = dao.AssignEmployeeToProject(1, 1);

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, GetRowCount("project_employee"));
        }

        [TestMethod]
        public void RemoveEmployeeRemovesEmployeeFromProject()
        {
            //Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);

            //Act
            bool result = dao.RemoveEmployeeFromProject(1, 1);

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, GetRowCount("project_employee"));
        }

        [TestMethod]
        public void CreateProjectSuccessfullyIncrementsId()
        {
            //Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);
            Project project = new Project { Name = "Testing Stuff", StartDate = Convert.ToDateTime("10/19/2021"), EndDate = Convert.ToDateTime("10/19/2120") };

            //Act
            int id = dao.CreateProject(project);

            //Assert
            Assert.IsTrue(id > 1, "Project could not be added");
            Assert.AreEqual(2, GetRowCount("project"));
        }
    }
}
