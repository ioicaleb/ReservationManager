using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class SpaceSqlDAOTests : IntegrationTestBase
    {
        [TestMethod]
        public void GetSpacesReturnsDictionaryWithCorrectSpaces()
        {
            // Arrange
            SpaceDAO spaceDAO = new SpaceDAO(ConnectionString);
            VenueDAO venueDAO = new VenueDAO(ConnectionString);
            Dictionary<int, Venue> venues = venueDAO.GetVenues();
            Venue venue = venues[1];
            Dictionary<int, Space> spaces = new Dictionary<int, Space>();

            // Act
            spaces = spaceDAO.GetSpaces(venue);

            // Assert
            Assert.IsNotNull(spaces);
            Assert.AreEqual(2, spaces.Count());
            Assert.AreEqual("OpenTest", spaces[1].Name);
            Assert.AreEqual("RestrictedTest", spaces[2].Name);
        }

        [TestMethod]
        [DataRow("ALWAYS", 0)]
        [DataRow("Jan.", 1)]
        [DataRow("May", 5)]
        [DataRow("Sep.", 9)]
        [DataRow("NEVER", 13)]
        public void ChangeMonthToAbbrReturnsCorrectString(string expected, int month)
        {
            //Arrange
            SpaceDAO spaceDAO = new SpaceDAO(ConnectionString);

            //Act
            string result = spaceDAO.ChangeIntToMonthAbbr(month);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SearchOnlyReturnsCorrectSpaces()
        {
            // Arrange
            SpaceDAO spaceDAO = new SpaceDAO(ConnectionString);
            VenueDAO venueDAO = new VenueDAO(ConnectionString);
            Dictionary<int,Venue> venues = venueDAO.GetVenues();
            Venue venue = venues[1];
            Dictionary<int, Space> spaces = new Dictionary<int, Space>();
            DateTime date = DateTime.Parse("2021/07/07");

            // Act
            spaces = spaceDAO.SearchSpaces(venue, 30, date, 5, "None", 100000000, false);

            // Assert
            Assert.IsNotNull(spaces);
            Assert.AreEqual(1, spaces.Count());
            Assert.AreEqual("RestrictedTest", spaces[2].Name);
        }
    }
}
