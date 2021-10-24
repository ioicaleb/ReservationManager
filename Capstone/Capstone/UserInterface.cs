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
        private Space Space = new Space();
        private Reservation Reservation = new Reservation();

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
        /// Begins to call the displaying of the main menu options for user. If user chooses to view list of venues, a list is generated and venue submenu is called.
        /// A user can quit the program here also.
        /// </summary>
        public void Run()
        {
            bool repeat = true;
            while (repeat)
            {
                // Main Menu Inquiry takes in user response as a string and returns it into userInput.
                string userInput = CLIHelper.GetString("What would you like to do?:\n   1) List Venues\n   2) Search for a Space\n   3) Search for an existing reservation\n   Q) Quit\n").ToLower();

                // Switch user input for main menu selction
                switch (userInput)
                {
                    case "1":
                        Console.Clear();
                        DisplayVenueMenu();
                        Console.Clear();
                        break;
                    case "2":
                        DisplayDesiredSpaces();
                        Console.Clear();
                        break;
                    case "3":
                        SearchReservationDetails();
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
        /// Provides user with inquiries on budget, category, time, attendees to filter search and find ALL available spaces based on user needs.
        /// </summary>
        public void DisplayDesiredSpaces()
        {
            // Retrieves all venues and gathers user input to filter search.
            ICollection<Venue> venues = venueDAO.GetVenues();

            DateTime startDate = CLIHelper.GetDate();
            int stayLength = CLIHelper.GetInteger("How many days will you need the space?: ");
            int numberOfAttendees = CLIHelper.GetInteger("How many people will be in attendance?: ");
            bool needsAccessible = CLIHelper.GetBool("Does the space require accessibility accommodations (Y/N)?: ");
            string categoryList = String.Format("{0,4}{1}\n{2,4}{3}\n{4,4}{5}\n{6,4}{7}\n{8,4}{9}\n{10,4}{11}\n{12,4}{13}\n", "1)", "Family Friendly", "2)", "Outdoors", "3)", "Historic", "4)", "Rustic",
                "5)", "Luxury", "6)", "Modern", "7)", "None");
            int categoryIndex = CLIHelper.GetInteger("Which of the categories would you like to include?\n" + categoryList);
            string[] categories = new string[] { "Family Friendly", "Outdoors", "Historic", "Rustic", "Luxury", "Modern", "None" };
            // A new category is obtained by pulling from an array, representing a category in the list of options, selected by user.
            string category = categories[categoryIndex - 1];
            int budget = CLIHelper.GetInteger("What is your budget?: ");
            //Dictionary<int, Space> spaces = new Dictionary<int, Space>();
            Console.WriteLine("Here are the available spaces based on your requirements:");
            foreach (Venue venue in venues) // Looping through list of venues so it can associate a specific venue when comparing a category filter to user's choice within DAO.
            {
                //Venue = venue; // Setting the class feild of reusable Venue to the each venue in the list as it loops. Not necessary as the Venue is not being returned or passed in.
                Dictionary<int, Space> spacesToAdd = spaceDAO.SearchSpaces(venue, numberOfAttendees, startDate, stayLength, category, budget, needsAccessible);
                
                if (spacesToAdd.Count() > 0) // Check that list is not empty
                {
                    // Looping through the dictionary of spaces obtained based on user's requirements to...?
                    //foreach (KeyValuePair<int, Space> space in spacesToAdd)
                    //{
                    //    spaces[space.Key] = space.Value;
                    //}
                    DisplaySpaceDetails(spacesToAdd);
                    Console.WriteLine();
                }
                else if (spacesToAdd.Count() == 0) // If the dictionary is empty, the user will be prompt to try another search or to the main menu.
                {
                 bool valid = false;
                    while (!valid)
                    {
                        string userInput = CLIHelper.GetString("We're sorry, there are no available spaces based on the information you provided.\n  Y/N) Would you like to try another search?").ToLower();
                        if (userInput == "y")
                        {
                            DisplayDesiredSpaces();
                        }
                        else
                        {
                            Console.Clear();
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Displays a list of all venues for user to select from. Provides menu options for user selection.
        /// </summary>
        /// <returns></returns>
        public bool DisplayVenueMenu()
        {
            bool leaveMenu = false; 
            ICollection<Venue> venues = venueDAO.GetVenues();
            while (!leaveMenu)
            {
                // Loops through the collection to print details of each venue for selectable options.
                DisplayAllVenues(venues);

                int userInt = CLIHelper.GetInteger("Which select a menu option: ");

                // -1 represents "r", meaning user has chosen to return to the main menu.
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
                    // Venue now represents the user's chosen venue
                    Venue = Venue.GetSelectedVenue(userInt, venues);
                    leaveMenu = DisplayVenueSubMenu(); // A bool which will be used to determine whether or not this menu can be left or restarted based on output of method.
                }
            }
            return false;
        }

        /// <summary>
        /// Takes in a list of venues from selecion query in DAO.
        /// Displays all venues in the database as selectable options for user.
        /// </summary>
        /// <param name="venues"></param>
        public void DisplayAllVenues(ICollection<Venue> venues)
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
        public bool DisplayVenueSubMenu()
        {

            // Leave menu will leave the loop, otherwise, a later menu, may choose to return to this menu and restart the venue selection process.
            bool leaveMenu = false;
            while (!leaveMenu)
            {
                // Output the venue's ToStringed details to user
                Console.WriteLine(Venue);
                Console.WriteLine();

                string userInput = CLIHelper.GetString("What would you like to do next?\n" +
                "1) View Spaces\n2) View Upcoming Reservations\nR) Return to Previous Screen\n").ToLower();
                switch (userInput)
                {
                    case "1":
                        // Displays a list of spaces specific to the user's selected venue. Passing in the venue's Id.
                        // A dictionary of Key: ID and Value: space returned for user's selected venue.
                        Dictionary<int, Space> spaces = spaceDAO.GetSpaces(Venue);
                        Console.Clear();
                        leaveMenu = DisplaySpacesMenu(spaces);
                        break;
                    case "2":
                        DisplayUpcomingReservations();
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
        public bool DisplaySpacesMenu(Dictionary<int, Space> spaces)
        {
            bool valid = false;
            while (!valid)
            {
                // Loops through a dictionary to print each space in venue.
                DisplaySpaceDetails(spaces);
                Console.WriteLine();
                valid = HandleSpaceMenu(spaces);
            }
            return false;
        }

        /// <summary>
        /// Manages the spaces menu. Collects information and gets available spaces based on user input parameters.
        /// </summary>
        /// <param name="spaces"></param>
        /// <returns></returns>
        public bool HandleSpaceMenu(Dictionary<int, Space> spaces)
        {
            string userInput = CLIHelper.GetString("What would you like to do?\n    1) Reserve a Space\n    R) Return to the Previous Screen\n").ToLower();
            switch (userInput)
            {
                case "1":
                    DateTime startDate = CLIHelper.GetDate();
                    int stayLength = CLIHelper.GetInteger("How many days will you need the space?: ");
                    ICollection<int> spacesAvailable = reservationDAO.GetAvailableSpaces(Venue.Id, startDate, stayLength);
                    return DisplaySortedSpaces(spaces, spacesAvailable, startDate, stayLength);
                case "r":
                    Console.Clear();
                    return true;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid Input");
                    Console.WriteLine();
                    break;
            }
            return false;
        }
        /// <summary>
        /// Allows user to search for a list of upcoming reservations based on which venue they are in.
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        public bool SearchReservationDetails()
        {
            int reservationId = CLIHelper.GetInteger("What was your confirmation id?: ");
            Reservation = reservationDAO.SearchReservation(reservationId);
            Console.WriteLine(Reservation);
            Console.WriteLine();
            return true;
        }

        /// <summary>
        /// Searches ALL upcoming reservations for the next 30 days and displays them to the user.
        /// </summary>
        /// <returns></returns>
        public bool DisplayUpcomingReservations()
        {
            ICollection<Reservation> reservations = reservationDAO.GetUpcomingReservations(Venue.Id);
            Console.Clear();
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
        public bool DisplaySortedSpaces(Dictionary<int,Space> spaces, ICollection<int> spacesAvailable, DateTime startDate, int stayLength)
        {
            bool valid;
            // If no spaces are available based on user input, ask user to serach again.
            if (spacesAvailable.Count < 1)
            {
                Console.WriteLine("------NO SPACES AVAILABLE------");
                Console.WriteLine();
                return true;
            }
            DisplayAvailableSpaces(stayLength, spaces, spacesAvailable);
            valid = DisplayReservationMenu(spaces, startDate, stayLength);
            return valid;
        }

        /// <summary>
        /// Displays the details of the dictionary to console screen.
        /// </summary>
        /// <param name="spaces"></param>
        public void DisplaySpaceDetails(Dictionary<int, Space> spaces)
        {
            Console.WriteLine($"{Venue.Name} spaces");
            Console.WriteLine(String.Format("{0,4}{1,-33}{2,-9}{3,-9}{4,-13}{5}", "", "Name", "Open", "Close", "Daily Rate", "Max. Occupancy"));
            foreach (KeyValuePair<int, Space> space in spaces)
            {
                Console.WriteLine(String.Format("{0,4}{1,-33}{2,-9}{3,-9}{4,-13}{5}", space.Value.Id + ") ", space.Value.Name, space.Value.OpenMonth, space.Value.CloseMonth, space.Value.DailyRate.ToString("C"), space.Value.MaxOccupancy));
            }
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
        public bool DisplayReservationMenu(Dictionary<int, Space> spaces, DateTime startDate, int stayLength)
        {
            bool valid = false;
            while (!valid)
            {
                int spaceChoice = CLIHelper.GetInteger("\nWhich space would you like to reserve (enter 0 to cancel)?: ");
                if (spaceChoice == 0)
                {
                    Console.Clear();
                    return false; // this will allow looping back through spaces menu by when setting valid set to false.
                }
                // The number the user input must be a real space provided, if so, it will create a new reservation in that database.
                else if (spaces.ContainsKey(spaceChoice))
                {
                    Space = spaces[spaceChoice];

                    string reserver = CLIHelper.GetString("Who is this reservation for?: ");
                    Reservation = Reservation.GetReservationDetails(Space, startDate, stayLength, reserver);
                    int confirmationNumber = reservationDAO.ReserveSpace(Reservation);
                    Reservation.Id = confirmationNumber;
                    // Output the reservation confirmation information to the user on the screen including a confirmation number which represents a reservation ID.
                    PrintReservationConfirmation(Reservation);
                    Console.WriteLine();
                    return true;
                }
                else // The user's choice was < or > the available spaces...
                {
                    Console.WriteLine("Invalid input");
                    Console.WriteLine();
                }
            }
            return false;
        }

        // Printing out all of the reservation confirmation details for user's records.
        public void PrintReservationConfirmation(Reservation Reservation)
        {
            Console.WriteLine("Thanks for submitting your reservation! The details for your event are listed below: ");
            Console.WriteLine();
            string output = String.Format
            ("{0,16}{1}\n{2,16}{3}\n{4,16}{5}\n{6,16}{7}\n{8,16}{9}\n{10,16}{11}\n{12,16}{13}\n",
            "Confirmation #: ", Reservation.Id, "Venue: ", Reservation.VenueName, "Space: ", Reservation.SpaceName, "Reserved For: ", Reservation.ReservedBy, "Arrival Date: ",
                Reservation.StartDate.ToString("MM/dd/yyyy"), "Depart Date: ", Reservation.EndDate.ToString("MM/dd/yyyy"), "TotalCost: ", Reservation.TotalCost);
            Console.WriteLine(output);
            Console.WriteLine();
            Console.WriteLine("Press any key to continue (WARNING! This will remove the confirmation details)");
            Console.ReadKey();
            Console.Clear();
        }
    }
}