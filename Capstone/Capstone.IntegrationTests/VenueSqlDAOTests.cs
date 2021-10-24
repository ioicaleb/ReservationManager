using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class VenueSqlDAOTests : IntegrationTestBase
    {
        [TestMethod]
        public void GetVenuesReturnsCorrectNumberOfVenues()
        {
            // Arrange
            VenueDAO venueDAO = new VenueDAO(ConnectionString);

            // Act
            Dictionary<int, Venue> venues = venueDAO.GetVenues();

            // Assert
            Assert.IsNotNull(venues);
            Assert.AreEqual(1, venues.Count);
        }

        [TestMethod]
        public void GetVenuesReturnsCorrectVenueInformation()
        {
            // Arrange
            VenueDAO venueDAO = new VenueDAO(ConnectionString);

            // Act
            Dictionary<int, Venue> venues = venueDAO.GetVenues();

            // Assert
            Assert.AreEqual(1, venues[1].Id);
            Assert.AreEqual("Test", venues[1].Name);
            Assert.AreEqual("Test", venues[1].Description);
            Assert.AreEqual(2, venues[1].CityId);
        }

        [TestMethod]
        public void GetCategoryReturnsCorrectCategoryName()
        {
            // Arrange
            VenueDAO venueDAO = new VenueDAO(ConnectionString);
            Dictionary<int,Venue> venues = venueDAO.GetVenues();
            Venue venue = venues[1];

            // Act
            venue.Categories = venueDAO.GetCategory(1);
            string category1 = venue.Categories[0];
            string category2 = venue.Categories[1];

            // Assert
            Assert.AreEqual(2, venue.Categories.Count);
            Assert.AreEqual("Family Friendly", category1);
            Assert.AreEqual("Modern", category2);
        }
    }
}
