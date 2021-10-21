using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    /// <summary>
    /// This class is responsible for representing the main user interface to the user.
    /// </summary>
    /// <remarks>
    /// ALL Console.ReadLine and WriteLine in this class
    /// NONE in any other class. 
    ///  
    /// The only exceptions to this are:
    /// 1. Error handling in catch blocks
    /// 2. Input helper methods in the CLIHelper.cs file
    /// 3. Things your instructor explicitly says are fine
    /// 
    /// No database calls should exist in classes outside of DAO objects
    /// </remarks>
    public class UserInterface
    {
        private readonly string connectionString;

        private readonly IVenueDAO venueDAO;
        private readonly IReservationDAO reservationDAO;
        private readonly ISpaceDAO spaceDAO;

        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueDAO(connectionString);
            reservationDAO = new ReservationDAO(connectionString);
            spaceDAO = new SpaceDAO(connectionString);
        }

        public void Run()
        {
            // Display the header
            while (true)
            {
                // Display the menu and retrieve user input for menu selection.
                string userInput = DisplayMainMenu();
                Console.WriteLine();
                // switch statement for menu options
                switch (userInput)
                {
                    case "1":
                        // method within the venue DAL
                        ICollection<Venue> venues = GetVenues();
                        Console.WriteLine();
                        GetVenueMenu(venues);
                        break;
                        
                    case "q":
                        Console.WriteLine("Thank you for shopping with Excelsior Venues!");
                        return;
                    default:
                        Console.WriteLine("Please select an option from the menu.");
                        break;
                }
            }
        }

        /// <summary>
        /// Displays the menu of options to the user to list venues or quit the application.
        /// </summary>
        public string DisplayMainMenu()
        {
            Console.WriteLine("What would you like to do?:");
            Console.WriteLine("1) List Venues");
            Console.WriteLine("Q) Quit");
            return Console.ReadLine().ToLower();
        }

        /// <summary>
        /// Retrieves the list of the the venues from the database and displays the id and name to the user.
        /// </summary>
        public ICollection<Venue> GetVenues()
        {
            // Storing result of query into list variable in venueDAO, so it can be used to display.
            ICollection<Venue> venues = venueDAO.GetVenues();

            // Display the items based on the user's choice
            Console.WriteLine($"Here are the available venues: ");
            // Loop through the list of venues to display them to the user

            foreach (Venue venue in venues)
            {
                // Printing out the to string method for venue each loop
                Console.WriteLine($"{venue.Id}) {venue.Name}");
            }

            Console.WriteLine("R) Return to previous Screen");
            return venues;
        }

        public void GetVenueMenu(ICollection<Venue> venues)
        {
            // Spaces
            bool valid = false;
            while (!valid)
            {
                string userInput = Console.ReadLine();
                int intValue;
                if (userInput.ToLower() == "r")
                {
                    valid = true;
                    break;
                }
                else if (int.TryParse(userInput, out intValue))
                {
                    spaceDAO.GetSpaces(intValue);
                    valid = true;
                    break;
                }
                Console.WriteLine("Invalid input");
            }
        }

        // Reveals list of spaces within the venue selected
        public void GetSpaces(int venueId, string name)
        {
            //int venueId = CLIHelper.GetInteger("Select your venue: ");
            IEnumerable<Space> spaces = spaceDAO.GetSpaces(venueId);
            // Need to override to string for space
            Console.WriteLine($"{name} Spaces");
            // This will need changed to a better format
            Console.WriteLine("Name         Open  Close     Daily Rate       Max. Occupancy");
            foreach (Space space in spaces)
            {
                Console.WriteLine($"#{space.Id} {space.Name} {space.OpenMonth} {space.CloseMonth} {space.DailyRate} {space.MaxOccupancy}");
            }

            // Display a new submenu for user to choose what they would like to do with the spaces
            SpacesMenu();
        }

        public void SpacesMenu()
        {
            Console.WriteLine("1) Reserve a Space");
            Console.WriteLine("R) Return to the Previous Screen");
        }
        // Ability to select a space and search for availability so I can reserve the space.
        public void SearchSpaceAvailability()
        {

        }

        // Search availability for space selected
        public void AddReservation()
        {

        }

        // Try parsing the int which is in put by the user selected from venue list.
        //int venueID = CLIHelper.GetInteger("Select from the venue options: ");

        // Inquire to user about spaces
    }
}
