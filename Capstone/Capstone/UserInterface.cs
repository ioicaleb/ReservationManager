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

        // Instantiated new DAOs to allow them to take in a db connection string via constructor, and to call throughout class.
        private readonly IVenueDAO venueDAO;
        private readonly IReservationDAO reservationDAO;
        private readonly ISpaceDAO spaceDAO;
        private Venue Venue = new Venue();
        private Space space = new Space();

        /// <summary>
        /// Constructor takes in a database connectionString to pass it into each declaration of data access objects for connection purposes.
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
        /// Begins to call the displays of the main menu options for user. If user chooses to view list of venues, list is generated and Venue submenu is called.
        /// A user can quit the program here also.
        /// </summary>
        public void Run()
        {
            bool repeat = true;
            while (repeat)
            {
                // Main Menu Inquiry takes in user response as a string and returns it into userInput.
                string userInput = CLIHelper.GetString("What would you like to do?:\n   1) List Venues\n   2) Display Upcoming Reservations\n   Q) Quit\n").ToLower();

                // Switch user input from main menu selction
                switch (userInput)
                {
                    case "1":
                        Console.Clear();
                        DisplayVenueMenu();
                        break;
                    case "2":
                        ReservationSearchALL();
                        break;
                    case "q":
                        Console.WriteLine("Thank you for shopping with Excelsior Venues!");
                        repeat = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Please select an option from the menu.");
                        Console.WriteLine();
                        break;
                }
            }
        }

        /// <summary>
        /// Displays a list of venues for user to select from. Provides menu options for user selection.
        /// </summary>
        /// <returns></returns>
        public bool DisplayVenueMenu()
        {
            bool leaveMenu = false;
            // A list of venues generated from SQL query.
            ICollection<Venue> venues = venueDAO.GetVenues();
            while (!leaveMenu)
            {
                // Loops through the collection to print details of each venue for selectable options.
                DisplaysAllVenues(venues);

                int userInt = CLIHelper.GetInteger("Which select a menu option: "); //Normal user input, Sets to -1 if user selects R

                // -1 represents "r", user has chosen to return to the main menu.
                if (userInt == -1)
                {
                    Console.Clear();
                    leaveMenu = true;
                }
                // User has chosen a number to large or small to select a proper venue, takes them back through the loop.
                else if (userInt >= venues.Count() || userInt <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input. Venue not available");
                    Console.WriteLine();
                    leaveMenu = false;
                }
                // If user has correctly selected a venue. Now they can view the venue submenu.
                else
                {
                    Console.Clear();
                    leaveMenu = DisplayVenueSubMenu(userInt, venues);
                }
            }
            return false;
        }

        /// <summary>
        /// Takes in a list of venues from selecion query in DAO.
        /// Displays all venues in the database as selectable options for user.
        /// </summary>
        /// <param name="venues"></param>
        public void DisplaysAllVenues(ICollection<Venue> venues)
        {
            Console.WriteLine($"Which venue would you like to view?");
            // Loop through the list of venues to display them to the user
            foreach (Venue venue in venues)
            {
                Console.WriteLine(String.Format("{0,7}{1}", $"{venue.Id}) ", venue.Name));
            }
            Console.WriteLine(String.Format("{0,7}{1}", "R) ", "Return to previous Screen"));
            Console.WriteLine();
        }

        /// <summary>
        /// Display's a submenu within the specific venue selected inquiring if the user would like to view spaces or return to previous screen
        /// </summary>
        /// <param name="userInt"></param>
        /// <param name="venues"></param>
        /// <returns></returns>
        public bool DisplayVenueSubMenu(int userInt, ICollection<Venue> venues)
        {
            // Leave menu will leave the loop, otherwise, a later menu, may choose to return to this menu and restart the venue selection process.
            bool leaveMenu = false;
            while (!leaveMenu)
            {
                // Venue now represents the user's chosen venue
                Venue = Venue.GetSelectedVenue(userInt, venues);
                // Output the venue's ToStringed details to user
                Console.WriteLine(Venue);
                Console.WriteLine();

                string userInput = CLIHelper.GetString("What would you like to do next?\n" +
                "1) View Spaces\n2) Search for Reservations\nR) Return to Previous Screen\n").ToLower();
                switch (userInput)
                {
                    case "1":
                        // Displays a list of spaces specific to the user's selected venue. Passing in the venue's Id.
                        leaveMenu = DisplaySpacesMenu(Venue.Id);
                        break;
                    case "2":
                        leaveMenu = ReservationSearch(Venue.Id);
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
        /// A sub menu present within the spaces available from the Venue selected. Takes in a venue's Id to query database for spaces associated.
        /// The user can reserve a space or return to previous screen.
        /// </summary>
        public bool DisplaySpacesMenu(int venueId)
        {
            // A dictionary of Key: ID and Value: space returned for user's selected venue.
            Dictionary<int, Space> spaces = spaceDAO.GetSpaces(Venue);

            bool valid = false;
            while (!valid)
            {
                // Loops through a dictionary to print each space in venue.
                DisplaySpaceDetails(spaces);
                Console.WriteLine();
                string userInput = CLIHelper.GetString("What would you like to do?\n1) Reserve a Space\nR) Return to the Previous Screen\n").ToLower();
                switch (userInput)
                {
                    case "1":
                        valid = DisplaySortedSpaces(spaces);
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

        /// <summary>
        /// Allows user to search for a list of upcoming reservations based on which venue they are in.
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        public bool ReservationSearch(int venueId)
        {
            ICollection<Reservation> reservations = reservationDAO.GetNext30Days(venueId);
            Console.WriteLine("The following reservations are coming up in the next 30 days:");
            Console.WriteLine();
            Console.WriteLine(String.Format("{0,-33}{1,-33}{2,-18}{3,-12}{4}",
                "Venue", "Space", "Reserved For", "From", "To"));
            // write out each reservation
            foreach (Reservation reservation in reservations)
            {
                Console.WriteLine(reservation); //TostringOverride
            }
            Console.WriteLine();
            return true;
        }

        /// <summary>
        /// Searches ALL upcoming reservations for the next 30 days and displays them to the user.
        /// </summary>
        /// <returns></returns>
        public bool ReservationSearchALL()
        {
            ICollection<Reservation> reservations = reservationDAO.GetALLNext30Days();
            Console.WriteLine("The following reservations are coming up in the next 30 days:");
            Console.WriteLine();
            Console.WriteLine(String.Format("{0,-33}{1,-33}{2,-18}{3,-12}{4}",
                "Venue", "Space", "Reserved For", "From", "To"));
            // write out each reservation
            foreach (Reservation reservation in reservations)
            {
                Console.WriteLine(reservation); //TostringOverride
            }
            Console.WriteLine();
            return true;
        }

        /// <summary>
        /// Displays a list of all spaces from a specific venue for user to choose from.
        /// </summary>
        /// <param name="spaces"></param>
        public bool DisplaySortedSpaces(Dictionary<int, Space> spaces)
        {
            bool valid;
            DateTime startDate = GetDate();
            int stayLength = CLIHelper.GetInteger("How many days will you need the space?: ");
            int numberOfAttendees = CLIHelper.GetInteger("How many people will be in attendance?: ");
            ICollection<int> spacesAvailable = reservationDAO.GetAvailableSpaces(Venue.Id, startDate, stayLength, numberOfAttendees);
            DisplayAvailableSpaces(stayLength, spaces, spacesAvailable);
            valid = DisplayReservationMenu(spaces, startDate, stayLength, numberOfAttendees);
            return valid;
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

        /// <summary>
        /// The user must input the desired reservation date, which will be checked for correct format or user must retry.
        /// </summary>
        /// <returns></returns>
        public DateTime GetDate()
        {
            bool valid = false;
            Console.Clear();
            DateTime startDate = new DateTime();
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

        /// <summary>
        /// Spaces available based on the user's needs are printed to the console through this method.
        /// </summary>
        /// <param name="stayLength"></param>
        /// <param name="spaces"></param>
        /// <param name="spacesAvailable"></param>
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

        /// <summary>
        /// The user has been presented with a list of available spaces for reservation to choose from.
        /// This method provides the menu options for reserving a space, or to cancel and return to the spaces menu.
        /// </summary>
        /// <param name="spaces"></param>
        /// <param name="startDate"></param>
        /// <param name="stayLength"></param>
        /// <param name="numberOfAttendees"></param>
        /// <returns></returns>
        public bool DisplayReservationMenu(Dictionary<int, Space> spaces, DateTime startDate, int stayLength, int numberOfAttendees)
        {
            bool valid = false;
            while (!valid)
            {
                int spaceChoice = CLIHelper.GetInteger("\nWhich space would you like to reserve (enter 0 to cancel)?: ");
                if (spaceChoice == 0)
                {
                    Console.Clear();
                    return false; // this will allow looping back through spaces menu by keeping valid set to false.
                }
                // The number the user input must be a real space provided, if so, it will create a new reservation in that database.
                else if (spaces.ContainsKey(spaceChoice))
                {
                    string reserver = CLIHelper.GetString("Who is this reservation for?: ");
                    // Inserts a new reservation row into the DB reservation table
                    int confirmationNumber = reservationDAO.ReserveSpace(spaceChoice, numberOfAttendees, startDate, stayLength, reserver);
                    string spaceName = spaces[spaceChoice].Name;
                    string venueName = spaces[spaceChoice].VenueName;
                    string totalCost = spaces[spaceChoice].TotalCost.ToString("C");
                    // Output the reservation confirmation information to the user on the screen including a confirmation number which represents a reservation ID.
                    PrintReservationConfirmation(confirmationNumber, venueName, spaceName, reserver, numberOfAttendees, startDate, stayLength, totalCost);
                    Console.WriteLine();
                    return true;
                }
                else // The user's choice was < or > the available spaces...
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input");
                    Console.WriteLine();
                }
            }
            return false;
        }

        // Printing out all of the reservation confirmation details for user's records.
        public void PrintReservationConfirmation(int confirmationNumber, string venueName, string spaceName, string reserverName, int numberOfAttenddees, DateTime startDate, int stayLength, string totalCost)
        {
            Console.WriteLine("Thanks for submitting your reservation! The details for your event are listed below: ");
            Console.WriteLine(); 
            DateTime departDate = startDate.AddDays(stayLength);
            string output = String.Format
            ("{0,16}{1}\n{2,16}{3}\n{4,16}{5}\n{6,16}{7}\n{8,16}{9}\n{10,16}{11}\n{12,16}{13}\n{14,16}{15}",
            "Confirmation #: ", confirmationNumber, "Venue: ", venueName, "Space: ", spaceName, "Resrved For: ", reserverName, "Attendees: ", numberOfAttenddees, "Arrival Date: ", startDate.ToString("MM/dd/yyyy"), "Depart Date: ", departDate.ToString("MM/dd/yyyy"), "TotalCost: ", totalCost);
            Console.WriteLine(output);
            Console.WriteLine();
            Console.WriteLine("Press any key to continue (WARNING! This will remove the confirmation details)");
            Console.ReadKey();
            Console.Clear();
        }
    }
}

