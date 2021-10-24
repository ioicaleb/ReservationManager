using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capstone.Models
{
    /// <summary>
    /// This class represents a single venue, it has properties that will be set and pulled as well as specific format of being printed out.
    /// </summary>
    public class Venue
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public List<string> Categories { get; set; }


        /// <summary>
        /// Takes in user input for venue choice and a list of venues, converts list to array.
        /// Uses the user input to pick desired venue from array by index.
        /// </summary>
        /// <param name="venueId"></param>
        // Takes in the venueId which is parsed num input representing id of venue
        public Venue GetSelectedVenue(int venueId, Dictionary<int, Venue> venues)
        {
            Venue currVenue = venues[venueId];
            return currVenue;
        }

        // The default format for printing a venue
        public override string ToString()
        {
            return $"{Name}\n" +
                    $"Location: {Address}\n" +
                    $"Categories: {String.Join(", ", Categories)}\n\n" +
                    $"{Description}";
        }

    }

}
