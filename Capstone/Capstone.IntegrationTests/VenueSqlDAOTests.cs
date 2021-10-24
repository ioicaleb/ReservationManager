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
            ICollection<Venue> venues = venueDAO.GetVenues();

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
            ICollection<Venue> venues = venueDAO.GetVenues();
            Venue[] venuesArray = venues.ToArray();

            // Assert
            Assert.AreEqual(1, venuesArray[0].Id);
            Assert.AreEqual("Test", venuesArray[0].Name);
            Assert.AreEqual("Test", venuesArray[0].Description);
            Assert.AreEqual(2, venuesArray[0].CityId);
        }

        [TestMethod]
        public void GetCategoryReturnsCorrectCategoryName()
        {
            // Arrange
            VenueDAO venueDAO = new VenueDAO(ConnectionString);
            ICollection<Venue> venues = venueDAO.GetVenues();
            Venue[] venuesArray = venues.ToArray();
            Venue venue = venuesArray[0];

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
