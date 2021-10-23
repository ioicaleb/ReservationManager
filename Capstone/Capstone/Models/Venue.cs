using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capstone.Models
{
    public class Venue
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Categories { get; set; }


        /// <summary>
        /// Displaying details of the venue itself with some a submenu options
        /// </summary>
        /// <param name="venueId"></param>
        public Venue GetSelectedVenue(int venueId, ICollection<Venue> venues) // Takes in the venueId which is parsed num input representing id of venue
        {
            // Obtaining the values from the list as array indexes in order to pick apart and display accordingly.
            Venue[] venuesArr = venues.ToArray(); // To obtain values by index.

            Venue  currVenue = venuesArr[venueId - 1]; // Id in SQL is 1 more than the index

            return currVenue;
        }

        public override string ToString()
        {
            return $"{Name}\n" +
                    $"Location: {Address}\n" +
                    $"Categories: {Categories.Replace("|", ",").Replace(",  ", "")}\n\n" +
                    $"{Description}";
        }

    }

}
