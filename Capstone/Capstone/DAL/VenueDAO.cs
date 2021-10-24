using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class handles working with obtaining venues information from the database
    /// </summary>
    public class VenueDAO : IVenueDAO
    {
        // Query for GetVenues
        private const string SqlSelectVenues =
            "SELECT v.id, v.name, v.city_id, v.description, c.name + ', ' + c.state_abbreviation AS address " +
            "FROM venue v" +
            " INNER JOIN city c ON c.id = v.city_id " +
            "ORDER BY v.name";

        // Query for GetCategory
        private const string SqlSelectCategoryName =
            "SELECT c.name " +
            "FROM venue v INNER JOIN category_venue cv ON cv.venue_id = v.id INNER JOIN category c ON c.id = cv.category_id " +
            "WHERE v.id = @id";

        private readonly string connectionString;

        public VenueDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Selects all of the venues from the database returned as a dictionary of int, and venues.
        /// This is so they can be stored alphabetically while keeping a sequential number sequence.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Venue> GetVenues()
        {
            Dictionary<int, Venue> venues = new Dictionary<int, Venue>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectVenues, conn);

                    SqlDataReader reader = command.ExecuteReader();
                    int i = 1; // Starting the key off at at 1 and adding the next alphabetized venue from the reader.
                    while (reader.Read())
                    {
                        Venue venue = new Venue
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            CityId = Convert.ToInt32(reader["city_id"]),
                            Description = Convert.ToString(reader["description"]),
                            Address = Convert.ToString(reader["address"])
                        };
                        venues[i] = venue; // Representing key as an integer and assinging each venue to it in order.
                        i++;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying the database: " + ex.Message);
            }
            // Looping through
            foreach (KeyValuePair<int, Venue> venue in venues)
            {
                venue.Value.Categories = GetCategory(venue.Value.Id);
            }

            return venues;
        }

        /// <summary>
        /// This class is currently used for testing purposes, returns a list of categories for a specific venue
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        public List<string> GetCategory(int venueId)
        {
            List<string> category = new List<string>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectCategoryName, conn);
                    command.Parameters.AddWithValue("@id", venueId);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        category.Add(Convert.ToString(reader["name"]));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying the database: " + ex.Message);
            }
            return category;
        }
    }
}
