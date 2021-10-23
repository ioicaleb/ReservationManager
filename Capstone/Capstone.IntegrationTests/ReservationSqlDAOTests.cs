using Capstone.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class ReservationSqlDAOTests : IntegrationTestBase
    {
        [TestMethod]
        public void GetAvailableSpacesReturnsCorrectNumberOfSpaces()
        {
            // Arrange
            ReservationDAO reservationSqlDAO = new ReservationDAO(ConnectionString);
            List<int> spacesAvailable = new List<int>();
            DateTime startDate = DateTime.Parse("2021/05/10");

            // Act
            spacesAvailable = reservationSqlDAO.GetAvailableSpaces(1, startDate, 10, 1);
            // Assert
            Assert.IsNotNull(spacesAvailable);
            Assert.AreEqual(2, spacesAvailable.Count);
        }

        [TestMethod]
        public void GetAvailableSpacesReturnsCorrectIds()
        {
            // Arrange
            ReservationDAO reservationSqlDAO = new ReservationDAO(ConnectionString);
            List<int> spacesAvailable = new List<int>();
            DateTime startDate = DateTime.Parse("2021/05/10");

            // Act
            spacesAvailable = reservationSqlDAO.GetAvailableSpaces(1, startDate, 10, 1);
            // Assert
            Assert.AreEqual(1, spacesAvailable[0]);
            Assert.AreEqual(2, spacesAvailable[1]);
        }

        [TestMethod]
        public void ReserveSpaceCreatesReservationIdIfSpaceIsAvailabel()
        {
            // Arrange
            ReservationDAO reservationSqlDAO = new ReservationDAO(ConnectionString);
            DateTime startDate = DateTime.Parse("05/10/2021");

            // Act
            int result = reservationSqlDAO.ReserveSpace(1, 2, startDate, 10, "Test");

            // Assert
            Assert.AreEqual(2, result);
        }
    }
}
