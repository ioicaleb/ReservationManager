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
        private Venue Venue = new Venue();
        private Space space = new Space();

        /// <summary>
        /// Constructor takes in a database connectionString to pass into each declaration of data access objects.
        /// </summary>
        /// <param name="connectionString"></param>
        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueDAO(connectionString);
            reservationDAO = new ReservationDAO(connectionString);
            spaceDAO = new SpaceDAO(connectionString);
        }

        /// <summary>
        /// Begins display
        /// </summary>
        public void Run()
        {
            bool repeat = true;
            while (repeat)
            {
                // Display the menu and retrieve user input for menu selection.
                string userInput = CLIHelper.GetString("What would you like to do?:\n1) List Venues\nQ) Quit\n");
                // switch statement for menu options
                switch (userInput)
                {
                    case "1":
                        DisplayVenueMenu();
                        break;
                    case "q":
                        Console.WriteLine("Thank you for shopping with Excelsior Venues!");
                        repeat = false;
                        break;
                    default:
                        Console.WriteLine("Please select an option from the menu.");
                        break;
                }
            }
        }

        public bool DisplayVenueMenu()
        {
            bool leaveMenu = false;
            // Passing in the user input from having selected a venue along with the venues list
            ICollection<Venue> venues = venueDAO.GetVenues();
            while (!leaveMenu)
            {
                DisplaySelectedVenueDetails(venues);

                int userInt = CLIHelper.GetInteger("Which venue would you like to check?: "); //Normal user input, Sets to -1 if user selects R

                if (userInt == -1)
                {
                    Console.Clear();
                    leaveMenu = true;
                }
                else if (userInt >= venues.Count() || userInt <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input. Venue not available");
                    Console.WriteLine();
                    leaveMenu = false;
                }
                else
                {
                    Console.Clear();
                    leaveMenu = DisplayVenueSubMenu(userInt, venues);
                }
            }
            return false;
        }

        public void DisplaySelectedVenueDetails(ICollection<Venue> venues)
        {
            // Display the items based on the user's choice
            Console.WriteLine($"Here are the available venues: ");
            // Loop through the list of venues to display them to the user
            foreach (Venue venue in venues)
            {
                Console.WriteLine(String.Format("{0,4}{1}", $"{venue.Id}) ", venue.Name));
            }
            Console.WriteLine(String.Format("{0,4}{1}", "R) ", "Return to previous Screen"));
            Console.WriteLine();
        }

        public bool DisplayVenueSubMenu(int userInt, ICollection<Venue> venues)
        {

            bool leaveMenu = false;
            while (!leaveMenu)
            {
                Venue = Venue.GetSelectedVenue(userInt, venues);
                // Output the information to user
                Console.WriteLine(Venue);
                Console.WriteLine();
                // Switch based on user input for the submenu
                string userInput = CLIHelper.GetString("What would you like to do next?\n" +
                "1) View Spaces\n2) Search for Reservations\nR) Return to Previous Screen\n").ToLower(); // Calling the sub menu to appear
                switch (userInput)
                {
                    case "1":
                        leaveMenu = DisplaySpacesMenu(Venue.Id);
                        break;
                    case "2":
                        break;
                    case "r":
                        Console.Clear();
                        return false;
                    default:
                        Console.Clear();
                        Console.WriteLine("Please select an option from the menu.");
                        Console.WriteLine();
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// A sub menu present within the spaces available from the Venue selected.
        /// </summary>
        public bool DisplaySpacesMenu(int venueId) // This venue id has been passed down for generations
        {
            Dictionary<int, Space> spaces = spaceDAO.GetSpaces(Venue.Id);

            bool valid = false;
            while (!valid)
            {
                
                DisplaySpaceDetails(spaces);
                Console.WriteLine();
                string userInput = CLIHelper.GetString("What would you like to do?\n1) Reserve a Space\nR) Return to the Previous Screen\n").ToLower();

                switch (userInput)
                {
                    case "1":
                        DateTime startDate = GetDate();
                        int stayLength = CLIHelper.GetInteger("How many days will you need the space?: ");
                        int numberOfAttendees = CLIHelper.GetInteger("How many people will be in attendance?: ");
                        ICollection<int> spacesAvailable = reservationDAO.GetReservations(Venue.Id, startDate, stayLength, numberOfAttendees);
                        if (spacesAvailable.Count < 1)
                        {
                            return false;
                        }
                        DisplayAvailableSpaces(stayLength, spaces, spacesAvailable);
                        valid = DisplayReservationMenu(spaces, startDate, stayLength, numberOfAttendees);
                        break;
                    case "r":
                        Console.Clear();
                        return false;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid Input");
                        Console.WriteLine();
                        break;
                }
            }
            return true;
        }

        public void DisplaySpaceDetails(Dictionary<int, Space> spaces)
        {
            // This will need changed to a better format of spacing
            Console.Clear();
            Console.WriteLine($"{Venue.Name} spaces"); // Need to pull name from venue
            Console.WriteLine(String.Format("{0,4}{1,-33}{2,-9}{3,-9}{4,-13}{5}", "", "Name", "Open", "Close", "Daily Rate", "Max. Occupancy"));
            foreach (KeyValuePair<int, Space> space in spaces)
            {
                Console.WriteLine(String.Format("{0,4}{1,-33}{2,-9}{3,-9}{4,-13}{5}", space.Value.Id + ") ", space.Value.Name, space.Value.OpenMonth, space.Value.CloseMonth, space.Value.DailyRate.ToString("C"), space.Value.MaxOccupancy));
            }
        }
        public DateTime GetDate()
        {
            bool valid = false;
            Console.Clear();
            // Search for reservations available based on the needs of the customer
            DateTime startDate = new DateTime(01 / 01 / 2000);
            while (!valid)
            {
                string startDateInput = CLIHelper.GetString("When do you need the space? (YYYY/MM/DD) : ");
                // Datetime parse
                if (DateTime.TryParse(startDateInput, out startDate))
                {
                    valid = true;

                    break;
                }
                Console.WriteLine("Invalid input, check format.");
            }
            return startDate;
        }
        public void DisplayAvailableSpaces(int stayLength, Dictionary<int, Space> spaces, ICollection<int> spacesAvailable)
        {
            Console.WriteLine();
            Console.WriteLine("The following spaces are available based on your needs:");
            Console.WriteLine();
            Console.WriteLine(String.Format("{0,-10}{1,-33}{2,-13}{3,-12}{4,-13}{5,-12}",
                "Space #", "Name", "Daily Rate", "Max Occup.", "Accessible?", "Total Cost"));
            Space space = new Space();
            foreach (int spaceId in spacesAvailable)
            {
                space = spaces[spaceId];
                space.TotalCost = space.DailyRate * stayLength;
                Console.WriteLine(space);
            }
        }

        public bool DisplayReservationMenu(Dictionary<int, Space> spaces, DateTime startDate, int stayLength, int numberOfAttendees)
        {
            bool valid = false;
            while (!valid)
            {
                int spaceChoice = CLIHelper.GetInteger("\nWhich space would you like to reserve (enter 0 to cancel)?: ");
                if (spaceChoice == 0)
                {
                    Console.Clear();
                    return false; // Need to go back and cancel out of active reservation
                }
                else if (spaces.ContainsKey(spaceChoice))
                {
                    string reserver = CLIHelper.GetString("Who is this reservation for?: ");
                    int confirmationNumber = reservationDAO.ReserveSpace(spaceChoice, numberOfAttendees, startDate, stayLength, reserver);
                    string spaceName = spaces[spaceChoice].Name;
                    string venueName = spaces[spaceChoice].VenueName;
                    string totalCost = spaces[spaceChoice].TotalCost.ToString("C");
                    PrintReservationConfirmation(confirmationNumber, venueName, spaceName, reserver, numberOfAttendees, startDate, stayLength, totalCost);
                    Console.WriteLine();
                    return true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input");
                    Console.WriteLine();
                }
            }
            return false;
        }

        // Perform the reservation (ReserveSpace in DAO)
        public void PrintReservationConfirmation(int confirmationNumber, string venueName, string spaceName, string reserverName, int numberOfAttenddees, DateTime startDate, int stayLength, string totalCost)
        {
            Console.WriteLine("Thanks for submitting your reservation! The details for your event are listed below: ");
            Console.WriteLine(); DateTime departDate = startDate.AddDays(stayLength);
            string output = String.Format
            ("{0,16}{1}\n{2,16}{3}\n{4,16}{5}\n{6,16}{7}\n{8,16}{9}\n{10,16}{11}\n{12,16}{13}\n{14,16}{15}",
            "Confirmation #: ", confirmationNumber, "Venue: ", venueName, "Space: ", spaceName, "Resrved For: ", reserverName, "Attendees: ", numberOfAttenddees, "Arrival Date: ", startDate.ToString("MM/dd/YYYY"), "Depart Date: ", departDate.ToString("MM/dd/YYYY"), "TotalCost: ", totalCost);
            Console.WriteLine(output);
            Console.ReadKey();
        }
    }
}

