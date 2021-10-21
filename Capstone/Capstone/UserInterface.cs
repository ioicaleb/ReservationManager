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
        private readonly IVenueDAO venueDAO;
        private readonly IReservationDAO ReservationDAO;
        private readonly ISpaceDAO spaceDAO;

        private readonly string connectionString;

        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueDAO(connectionString);
        }

        public void Run()
        {
            // Display the header

            // Display the menu
            DisplayMainMenu();

            // Retrieve user input for menu selection.
            string userInput = Console.ReadLine().ToLower();

            while (true)
            {
                // switch statement for menu options
                switch (userInput)
                {
                    case "1":
                        // method within the venue DAL
                        DisplayAllVenues();
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

        public void DisplayMainMenu()
        {
            Console.WriteLine("1) List Venues");
            Console.WriteLine("q) Quit");
        }

        // Dictionary considered for storing venu list. This will be a method to select Venues.
        // For each venu revealed, a user may be able to choose to display all spaces for the venue.
        public void DisplayVenuList()
        {
            Console.WriteLine("1) Hidden Owl Eatery");
            // Ex: GetSpaces(); for this venue.
            Console.WriteLine("2) Painted Squirrel Club");
            Console.WriteLine("3) Rusty Farmer Spot");
            Console.WriteLine("4) Lilac Bottle Speakeasy");
            Console.WriteLine("5) Smirking Stone Bistro");
            Console.WriteLine("6) Blue Nomad Outpost");
            Console.WriteLine("7) Howling Pour Lounge");
            Console.WriteLine("8) Feisty Barrel Saloon");
            Console.WriteLine("9) Proud Lion Hideout");
            Console.WriteLine("10) Crystal Traveler Taproom");
            Console.WriteLine("11) Singing Table Pub");
            Console.WriteLine("12) Curious Anchor Garage");
            Console.WriteLine("13) Wise Rooster Brewhouse");
            Console.WriteLine("14) Runaway Time Emporium");
            Console.WriteLine("15) The Bittersweet Elephant Tavern");
            Console.WriteLine("R) Return to Previous Screen");
        }

        // Create a version of the methods within the DAL to tryparse user input
        // Select distinct FROM venu order by venue include various categories
        public void GetUniqueVenues()
        {
            IEnumerable<Venue> venues = VenueDAO.GetUniqueVenues();
        }

        // Considering spaces to be retrieved as a JOIN from venue DAL
        public void GetSpaces()
        {

        }

        // Ability to select a space and search for availability so I can reserve the space.
        public void SearchSpaceAvailability()
        {

        }

        // Search availability for space selected
        public void AddReservation()
        {

        }
    }
}
