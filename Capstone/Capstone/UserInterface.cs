using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // Instantiated new DAOs to allow them to take in a db connection string
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
                        // A new console readline overrides og.
                        userInput = Console.ReadLine(); //Normal user input

                        // Passing in the user input from having selected a venue along with the venues list
                        GetVenueMenu(userInput, venues);
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
            // Storing result of query into list variable in venueDAO, so it can be used to display here.
            ICollection<Venue> venues = venueDAO.GetVenues();

            // Display the items based on the user's choice
            Console.WriteLine($"Here are the available venues: ");

            // Loop through the list of venues to display them to the user
            foreach (Venue venue in venues)
            {
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
                //string userInput = Console.ReadLine();
                int intValue;
                if (userInput.ToLower() == "r")
                {
                    valid = true;
                    break;
                }
                else if (int.TryParse(userInput, out intValue))
                {
                    Console.Clear();
                    DisplayVenueDetails(intValue, venues);
                    valid = true;
                    break;
                }
                Console.WriteLine("Invalid input");
            }
        }

        /// <summary>
        /// Displaying details of the venue itself with some a submenu options
        /// </summary>
        /// <param name="venueId"></param>
        public void DisplayVenueDetails(int venueId, ICollection<Venue> venues) // Takes in the venueId which is parsed num input representing id of venue
        {
            // Obtaining the values from the list as array indexes in order to pick apart and display accordingly.
            Venue[] venuesArr = venues.ToArray(); // To obtain values by index.

            Venue currVenue = venuesArr[venueId - 1]; // Id in SQL is 1 more than the index

            // Output the information to user
            Console.WriteLine(currVenue);
            Console.WriteLine();
            VenueSubMenu(venueId, currVenue);
        }
        
        public void VenueSubMenu(int venueId, Venue venue)
        {
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("1) View Spaces");
            Console.WriteLine("2) Search for Reservations");
            Console.WriteLine("R) Return to Previous Screen");
            string userInput = Console.ReadLine().ToLower(); // Calling the sub menue to appear
            // Switch based on user input for the submenu
            switch (userInput)
            {
                case "1":
                    // VenueId arg has been passed into each method connected to this one to retiain id and limit size of methods.
                    GetSpaces(venueId, venue); 
                    break;
                case "2":
                    
                    break;
                case "R":
                    GetVenues();
                    break;
                default:
                    Console.WriteLine("Please select an option from the menu.");
                    break;
            }
        }

        // Reveals list of spaces within the venue selected
        public void GetSpaces(int venueId, Venue venue)
        {
            Dictionary<int, Space> spaces = spaceDAO.GetSpaces(venueId);
            // This will need changed to a better format of spacing
            Console.Clear();
            Console.WriteLine($"{venue.Name} spaces"); // Need to pull name from venue
            Console.WriteLine("Name     Open        Close       Daily Rate      Max. Occupancy");
            foreach (KeyValuePair<int, Space> space in spaces)
            {
                Console.WriteLine($"#{space.Value.Id} {space.Value.Name} {space.Value.OpenMonth} {space.Value.CloseMonth} {space.Value.DailyRate.ToString("C")} {space.Value.MaxOccupancy}");
            }

            // Display a new submenu for user to choose what they would like to do with the spaces
            SpacesMenu(spaces, venueId); // In order for spaces to pass venueId to get reservations, must pass in
        }

        /// <summary>
        /// A sub menu present within the spaces available from the Venue selected.
        /// </summary>
        public void SpacesMenu(Dictionary<int, Space> spaces, int venueId) // This venue id has been passed down for generations
        {
            Console.WriteLine();
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1) Reserve a Space");
            Console.WriteLine("R) Return to the Previous Screen");
            string userInput = Console.ReadLine().ToLower();

            switch (userInput)
            {
                case "1":
                    // Search for reservations available based on the needs of the customer.
                    // Obtaining parameters by gathering user info
                    DateTime startDate = new DateTime(01 / 01 / 2000);
                    // Datetime parse
                    bool valid = false;
                    while (!valid)
                    {
                        Console.WriteLine("When do you need the space? (YYYY/MM/DD) : ");
                        string startDateInput = Console.ReadLine();

                        if (DateTime.TryParse(startDateInput, out startDate))
                        {
                            valid = true;

                            break;
                        }

                        Console.WriteLine("Invalid input, check format.");
                    }

                    int stayLength = CLIHelper.GetInteger("How many days will you need the space?: ");
                    int numberOfAttendees = CLIHelper.GetInteger("How many people will be in attendance?: ");
                    // Getting a list of ints representing the ids of the spaces available to reserve
                    ICollection<int> spacesAvailable = reservationDAO.GetReservations(venueId, startDate, stayLength, numberOfAttendees);

                    Console.WriteLine();
                    Console.WriteLine("The following spaces are available based on your needs:");
                    Console.WriteLine();
                    DisplayAvailableSpaces(stayLength, spaces, spacesAvailable);
                    int spaceChoice = CLIHelper.GetInteger("Which space would you like to reserve (enter 0 to cancel)?: ");
                    if (spaceChoice == 0)
                    {
                        break; // Need to go back and cancel out of active reservation
                    }
                    string reserver = CLIHelper.GetString("Who is this reservation for?: ");

                    int confirmationNumber = reservationDAO.ReserveSpace(spaceChoice, numberOfAttendees, startDate, stayLength, reserver);
                    string spaceName = spaces[spaceChoice].Name;
                    string venueName = spaces[spaceChoice].VenueName;
                    string totalCost = spaces[spaceChoice].TotalCost.ToString("C");
                    PrintReservationConfirmation(confirmationNumber, venueName, spaceName, reserver, numberOfAttendees, startDate, stayLength, totalCost);
                    break;
                case "r":
                    break;
            }
        }

        public void DisplayAvailableSpaces(int stayLength, Dictionary<int, Space> spaces, ICollection<int> spacesAvailable)
        {
            // Loops through a dictionary of spaces (int id and space) to output each space available.
            Space space = new Space();
            foreach (int spaceId in spacesAvailable)
            {
                space = spaces[spaceId];
                space.TotalCost = space.DailyRate * stayLength;
                Console.WriteLine(space);
            }
        }

        // Perform the reservation (ReserveSpace in DAO)
        public void PrintReservationConfirmation(int confirmationNumber, string venueName, string spaceName, string reserverName, int numberOfAttenddees, DateTime startDate, int stayLength, string totalCost)
        {
            Console.WriteLine("Thanks for submitting your reservation! The details for your event are listed below: ");
            Console.WriteLine();DateTime departDate = startDate.AddDays(stayLength);
            string output = String.Format
            ("{0,-16}{1} \n {2,-7},{3} \n {4,7},{5} \n {6,13},{7} \n {8,11}{9} \n {10,14}{11}, \n {12,14}{13} \n {14,11}{15}",
            "Confirmation #: ",confirmationNumber, "Venue: ", venueName, "Space: ", spaceName, "Resrved For: ", reserverName, "Attendees: ", numberOfAttenddees, "Arrival Date: ", startDate, "Depart Date: ", departDate,  "TotalCost: ", totalCost);
            Console.WriteLine();
        }
    }
}

