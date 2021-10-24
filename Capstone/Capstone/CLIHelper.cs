using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Capstone
{
    /// <summary>
    /// This class contains helper methods that should help get valid input from users.
    /// </summary>
    public static class CLIHelper
    {
        public static int GetInteger(string message)
        {
            string userInput;
            int intValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
                if (userInput.ToLower() == "r")
                {
                    Console.WriteLine();
                    return -1;
                }
            }
            while (!int.TryParse(userInput, out intValue) || intValue < 1);
            Console.WriteLine();
            return intValue;
        }

        public static bool GetBool(string message)
        {
            string userInput;
            bool boolValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
                if (userInput.ToLower() == "y" || userInput.ToLower() == "yes")
                {
                    userInput = "true";
                }
                else if (userInput.ToLower() == "n" || userInput.ToLower() == "no")
                {
                    userInput = "false";
                }
            }
            while (!bool.TryParse(userInput, out boolValue));

            Console.WriteLine();
            return boolValue;
        }

        public static string GetString(string message)
        {
            string userInput;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message);
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (string.IsNullOrEmpty(userInput));

            Console.WriteLine();
            return userInput;
        }

        public static DateTime GetDate()
        {
            bool valid = false;
            Console.Clear();
            // Search for reservations available based on the needs of the customer
            DateTime startDate = new DateTime();
            while (!valid)
            {
                string startDateInput = CLIHelper.GetString("When do you need the space? (MM/DD/YYYY) : ");
                // Datetime parse
                CultureInfo enUs = new CultureInfo("en-US");
                if (DateTime.TryParseExact(startDateInput, "MM/dd/yyyy", enUs , DateTimeStyles.None, out startDate) && startDate > DateTime.Now)
                {
                    valid = true;

                    break;
                }
                Console.WriteLine("Invalid input, check format and that date has not passed and is not today.");
            }
            return startDate;
        }
    }
}
