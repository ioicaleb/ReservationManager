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
                        GetVenuesList();
                        Console.WriteLine();
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

        public string DisplayMainMenu()
        {
            Console.WriteLine("What would you like to do?:");
            Console.WriteLine("1) List Venues");
            Console.WriteLine("Q) Quit");
            return Console.ReadLine().ToLower();
        }

        // Create a version of the methods within the DAL to tryparse user input
        // Select distinct FROM venu order by venue include various categories
        public void GetVenuesList()
        {
            // Storing result of query into list variable, so it can be used to display what user has chosen.
            IEnumerable<Venue> venues = venueDAO.GetVenues();

            // Display the items based on the user's choice
            Console.WriteLine($"Here are the available venues: ");
            // Loop through the list of venues to display them to the user

            foreach (Venue venue in venues)
            {
                // Printing out the to string method for venue each loop
                Console.WriteLine($"{venue.Id}) {venue.Name}");
            }
        }

        // Reveals list of spaces within the venue selected
        public void GetSpaces()
        {
            //IEnumerable<Space> spaces = spaceDAO.GetSpaces();
            // CLI helper to convert the user's
            int venueID = CLIHelper.GetInteger("Select from the venue options: ");
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
    }
}
