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

        // Instantiated a new DAO
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
                        // Return list of venues from GetVenues method
                        ICollection<Venue> venues = GetVenues();
                        Console.WriteLine();

                        // Taking user input
                        userInput = Console.ReadLine();

                        // Passing in the user input from having selected a venue along with the venues list
                        GetVenueMenu(userInput, venues);
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
        /// Retrieves the list of the the venues from the database and displays the id and name to the user to select.
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

        /// <summary>
        /// A user's input is being parsed to select a particular venue. It then reveals venue details and a submenu.
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="venues"></param>
        public void GetVenueMenu(string userInput, ICollection<Venue> venues)
        {
            // Spaces
            bool valid = false;
            while (!valid)
            {
                userInput = Console.ReadLine();
                int intValue;
                if (userInput.ToLower() == "r")
                {
                    valid = true;
                    break;
                }
                else if (int.TryParse(userInput, out intValue))
                {
                    Console.Clear();
                    DisplayVenuDetails(intValue);
                    valid = true;
                    break;
                }
                Console.WriteLine("Invalid input");
            }
        }

        /// <summary>
        /// Details displaying of the Venue itself with some corresponding details
        /// </summary>
        /// <param name="venueId"></param>
        public void DisplayVenuDetails(int venueId) // Takes in the venueId which is parsed num input representing id of venue
        {
            //select * from venue where id = @id;
            //@id, venuId
            //passed in is intValue which is input input

            // Based on venue chosen from the user, displays details about the venue.
            foreach (Venue venu in venues) // How to pull venue
            //Name
            //Location
            //Categories
            Console.WriteLine();
            //Description
            Console.WriteLine();
            //Submenu
            string userInput = VenueSubMenu(); // Must relocate this to be outside of the method
            switch (userInput)
            {
                case "1":
                    ViewSpaces();
                    break;
                case "2":
                    ReservationSearch();
                    break;
                case "1":
                    GetVenues();
                    break;
            }
        }

        public string VenueSubMenu()
        {
            Console.WriteLine("1) View Spaces");
            Console.WriteLine("2) Search for Reservation");
            Console.WriteLine("R) Return to Previous Screen");
            return Console.ReadLine().ToLower();
        }

        // Reveals list of spaces within the venue selected
        public void GetSpaces(int venueId)
        {
            //int venueId = CLIHelper.GetInteger("Select your venue: ");
            IEnumerable<Space> spaces = spaceDAO.GetSpaces(venueId);
            // Need to override to string for space
            //Console.WriteLine($"{name} Spaces");
            // This will need changed to a better format
            Console.WriteLine("Name         Open  Close     Daily Rate       Max. Occupancy");
            foreach (Space space in spaces)
            {
                Console.WriteLine($"#{space.Id} {space.Name} {space.OpenMonth} {space.CloseMonth} {space.DailyRate} {space.MaxOccupancy}");
            }
            // Displaying the menu items within the spaces, gets user input and stores it into a string
        }

        /// <summary>
        /// A sub menu present within the spaces available from the Venue selected.
        /// </summary>
        public string SpacesMenu()
        {
            Console.WriteLine("1) Reserve a Space");
            Console.WriteLine("R) Return to the Previous Screen");
            return Console.ReadLine().ToLower();
        }

        // Display information about the space being reserved
        public void ReserveSpace()
        {
            string date = CLIHelper.GetString("When do you need the space?: ");
            int reservationDuration = CLIHelper.GetInteger("How many days will you need the space?: ");
            int numberOfAttendees = CLIHelper.GetInteger("How many people will be in attendance?: ");
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
