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
                // DisplayMainMenu takes in user input as a string and returns it into userInput.
                string userInput = DisplayMainMenu();
                Console.WriteLine();
                switch (userInput)
                {

                    case "1":
                        // GetVenueMenu displays menu options and returns a list of venues into the new venues variable.
                        ICollection<Venue> venues = GetVenueMenu();
                        Console.WriteLine();

                        bool valid = false;
                        while (!valid)
                        {
                            // User response to GetVenueMenu. If r, user wishes to return to main menu.
                            userInput = CLIHelper.GetString("Which option will you select? ");
                            int intValue;
                            if (userInput.ToLower() == "r")
                            {
                                valid = true;
                            }
                            else if (int.TryParse(userInput, out intValue))
                            {
                                Console.Clear();
                                // intValue is the user's input parsed into an int.
                                Venue = DisplayVenueDetails(intValue, venues);
                                DisplayVenueSubMenu();
                                valid = true;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input");
                            }
                        }
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

        /// <summary>
        /// The priting of the main menu of options.
        /// </summary>
        public string DisplayMainMenu()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1) List Venues");
            Console.WriteLine("Q) Quit");
            return Console.ReadLine().ToLower();
        }

        /// <summary>
        /// Retrieves the list of the the venues from the database and displays the id and name to the user for selection.
        /// </summary>
        public ICollection<Venue> GetVenueMenu()
        {
            // Storing result of sql query into a list Venues, so it can be used to display using the loop.
            ICollection<Venue> venues = venueDAO.GetVenues();
            Console.WriteLine($"Which venue would you like to view?");
            foreach (Venue venue in venues)
            {
                Console.WriteLine(String.Format("{0,-4}{1}", $"{venue.Id})", venue.Name));
            }
            Console.WriteLine(String.Format("{0,-4}{1}", "R)", "Return to previous Screen"));
            return venues;
        }

        /// <summary>
        /// Displays the details of the selected venue with some a submenu options.
        /// </summary>
        /// <param name="venueId"></param>
        public Venue DisplayVenueDetails(int venueId, ICollection<Venue> venues)
        {
            // Converting the list of all venues into to an array, to obtain user's choice by index.
            Venue[] venuesArr = venues.ToArray();

            // venueId is the user's parsed value as input. Making sure user input is not out of range.
            if (venueId > 0 && venueId < venuesArr.Length)
            {
                Venue = venuesArr[venueId - 1];
                // Outputs the ToStringed version of the Venue to the user, by obtaining it from an array by index. The index of the venue is the SQL Venue.venue_id - 1.
                Console.WriteLine(Venue);
                Console.WriteLine();
                return Venue;
            }
            else
            {
                Console.WriteLine("Please select from the options above.");
            }

            return Venue;
        }

        /// <summary>
        /// Displays the menu to appear within the selected venue screen. Provides options to view spaces, search for reservations or return to previous screen.
        /// </summary>
        public void DisplayVenueSubMenu()
        {
            bool valid = false;
            while (!valid)
            {
                // Using CLI Helper, outputs a message where a user can respond by inputing a response.
                string userInput = CLIHelper.GetString("What would you like to do next?\n" +
                "1) View Spaces\n2) Search for Reservations\nR) Return to Previous Screen\n").ToLower();
                switch (userInput)
                {
                    case "1": // View Spaces
                        GetSpaces();
                        valid = true;
                        break;
                    case "2":
                        valid = true;
                        break;
                    case "r":
                        valid = true;
                        break;
                    default:
                        Console.WriteLine("Please select an option from the menu.");
                        break;
                }
            }

        }

        // Reveals list of spaces within the venue selected
        public void GetSpaces()
        {
            Dictionary<int, Space> spaces = spaceDAO.GetSpaces(Venue.Id);
            // This will need changed to a better format of spacing
            Console.Clear();
            Console.WriteLine($"{Venue.Name} spaces"); // Need to pull name from venue
            Console.WriteLine(String.Format("{0,-5}{1,-33}{2,-9}{3,-9}{4,-13}{5}", "", "Name", "Open", "Close", "Daily Rate", "Max. Occupancy"));
            foreach (KeyValuePair<int, Space> space in spaces)
            {
                Console.WriteLine(String.Format("{0,-5}{1,-33}{2,-9}{3,-9}{4,-13}{5}", "#" + space.Value.Id, space.Value.Name, space.Value.OpenMonth, space.Value.CloseMonth, space.Value.DailyRate.ToString("C"), space.Value.MaxOccupancy));
            }
            // Display a new submenu for user to choose what they would like to do with the spaces
            SpacesMenu(spaces, Venue.Id); // In order for spaces to pass venueId to get reservations, must pass in
        }


        /// <summary>
        /// A sub menu present within the list of spaces from the Venue selected. The user can choose to reserve a space or return to the previous screen.
        /// </summary>
        public void SpacesMenu(Dictionary<int, Space> spaces, int venueId) // This venue id has been passed down for generations
        {
            bool valid = false;
            while (!valid)
            {
                Console.WriteLine();
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1) Reserve a Space");
                Console.WriteLine("R) Return to the Previous Screen");
                string userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    // User input gathered to determine availability of space based on customer's needs.
                    case "1":
                        DateTime startDate = new DateTime();
                        while (!valid)
                        {
                            Console.Write("When do you need the space? (YYYY/MM/DD): ");
                            string startDateInput = Console.ReadLine();

                            // Parsing user input for a start reservation period to check if it is a correct date format
                            if (DateTime.TryParse(startDateInput, out startDate))
                            {
                                valid = true;

                                break;
                            }
                            Console.WriteLine("Invalid input, check format.");
                        }
                        int stayLength = CLIHelper.GetInteger("How many days will you need the space?: ");
                        int numberOfAttendees = CLIHelper.GetInteger("How many people will be in attendance?: ");

                        // A list of spaces gathered based on the requirements of the user. If there are no spaces available, it will ask the user to search again or return to previous menu.
                        ICollection<int> spacesAvailable = reservationDAO.GetReservations(Venue.Id, startDate, stayLength, numberOfAttendees);
                        bool mainMenu = AvailableSpacesCheck(spacesAvailable);
                        if (mainMenu)
                        {
                            return;
                        }
                        DisplayAvailableSpaces(stayLength, spaces, spacesAvailable);
                        int spaceChoice = CLIHelper.GetInteger("\nWhich space would you like to reserve (enter 0 to cancel)?: ");

                        bool isValid = false;
                        // Checking that the dictionary does contain entered Id of one of the spaces listed.
                        if (!spaces.ContainsKey(spaceChoice) && !isValid)
                        {
                            // Keep asking the user for their int response to the inquiry regarding which space to reserve.
                            do
                            {
                                // Need to check user is trying to cancel the reservation.
                                if (spaceChoice == 0)
                                {
                                    isValid = true;
                                    Console.Clear();
                                    return;
                                }
                                Console.WriteLine("Please select a Space Id from the list.");
                                spaceChoice = CLIHelper.GetInteger("Which space would you like to reserve (enter 0 to cancel)?: ");

                            } while (!spaces.ContainsKey(spaceChoice) && !isValid);
                        }
                        string reserver = CLIHelper.GetString("Who is this reservation for?: ");
                        // Check for spaceChoice existing in the curren lsit of spaces
                        int confirmationNumber = reservationDAO.ReserveSpace(spaceChoice, numberOfAttendees, startDate, stayLength, reserver);
                        string spaceName = spaces[spaceChoice].Name;
                        string venueName = spaces[spaceChoice].VenueName;
                        string totalCost = spaces[spaceChoice].TotalCost.ToString("C");
                        PrintReservationConfirmation(confirmationNumber, venueName, spaceName, reserver, numberOfAttendees, startDate, stayLength, totalCost);
                        break;
                    case "r":
                        break;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
        }

        /// <summary>
        /// Checks that there is at least one available space to choose from, otherwise, provide alternate menu options for customer.
        /// </summary>
        /// <param name="spacesAvailable"></param>
        public bool AvailableSpacesCheck(ICollection<int> spacesAvailable)
        {
            int intValue = 0;
            bool mainMenuSelect = false;

            if (spacesAvailable.Count == 0)
            {
                bool isValid = false;
                while (!isValid)
                {
                    string userChoice = CLIHelper.GetString("\nWe're sorry, there are no available spaces for the required time and occupence you require.\n\n    1) Would you like to try a different space?: \n    R) Main Menu\n\nWhat would you like to do?: ");

                    if (userChoice.ToLower() == "r")
                    {
                        isValid = true;
                        Console.Clear();
                        mainMenuSelect = true;
                        // Issue of looping back to here aside from isValid being true which will prevent any further action.
                    }
                    else if (int.TryParse(userChoice, out intValue))
                    {
                        isValid = true;
                        if (isValid == true)
                        {
                            switch (intValue)
                            {
                                case 1:
                                    Console.Clear();
                                    // bring the user back to the spaces options
                                    GetSpaces();
                                    break;
                                default:
                                    Console.WriteLine("Please select an option from the menu.");
                                    break;
                            }
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("Invalid input");
                    }
                }
            }

            return mainMenuSelect;
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

        // Perform the reservation (ReserveSpace in DAO)
        public void PrintReservationConfirmation(int confirmationNumber, string venueName, string spaceName, string reserverName, int numberOfAttenddees, DateTime startDate, int stayLength, string totalCost)
        {
            Console.WriteLine("Thanks for submitting your reservation! The details for your event are listed below: ");
            Console.WriteLine(); DateTime departDate = startDate.AddDays(stayLength);
            string output = String.Format
            ("{0,-16}{1} \n {2,-7},{3} \n {4,7},{5} \n {6,13},{7} \n {8,11}{9} \n {10,14}{11}, \n {12,14}{13} \n {14,11}{15}",
            "Confirmation #: ", confirmationNumber, "Venue: ", venueName, "Space: ", spaceName, "Resrved For: ", reserverName, "Attendees: ", numberOfAttenddees, "Arrival Date: ", startDate, "Depart Date: ", departDate, "TotalCost: ", totalCost);
            Console.WriteLine();
        }
    }
}

