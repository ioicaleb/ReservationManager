using Capstone.DAL;
using Capstone.Models;
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
            spacesAvailable = reservationSqlDAO.GetAvailableSpaces(1, startDate, 1);
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
            DateTime startDate = DateTime.Parse("2021/01/10");

            // Act
            spacesAvailable = reservationSqlDAO.GetAvailableSpaces(1, startDate, 10);

            // Assert
            Assert.AreEqual(1, spacesAvailable[0]);
        }

        [TestMethod]
        public void ReserveSpaceCreatesReservationIdIfSpaceIsAvailable()
        {
            // Arrange
            ReservationDAO reservationSqlDAO = new ReservationDAO(ConnectionString);
            DateTime startDate = DateTime.Parse("05/10/2021");
            Reservation reservation = new Reservation
            {
                SpaceId = 1,
                StartDate = startDate,
                EndDate = startDate.AddDays(10),
                ReservedBy = "Test"
            };

        // Act
            int result = reservationSqlDAO.ReserveSpace(reservation);

            // Assert
            Assert.AreEqual(2, result);
        }
    }
}
