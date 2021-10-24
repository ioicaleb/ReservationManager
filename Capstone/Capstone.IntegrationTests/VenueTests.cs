using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class VenueTests
    {
        [TestMethod]
        public void GetSelectedVenueReturnsCorrectVenue()
        {
            //Arrange
            Venue venue = new Venue
            {
                Id = 1,
                Name = "Test"
            };
            List<Venue> venues = new List<Venue>();
            venues.Add(venue);

            //Act
            Venue result = venue.GetSelectedVenue(1, venues);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Test", result.Name);
        }
    }
}
