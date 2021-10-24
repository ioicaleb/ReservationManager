using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class handles working with Venues in the database.
    /// </summary>
    public class VenueDAO : IVenueDAO
    {
        private const string SqlSelectVenues =
            "SELECT v.id, v.name, v.city_id, v.description, c.name + ', ' + c.state_abbreviation AS address " +
            "FROM venue v INNER JOIN city c ON c.id = v.city_id " +
            "ORDER BY v.name";


        private const string SqlSelectCategoryName =
            "SELECT c.name " +
            "FROM venue v INNER JOIN category_venue cv ON cv.venue_id = v.id INNER JOIN category c ON c.id = cv.category_id " +
            "WHERE v.id = @id";

        private readonly string connectionString;

        public VenueDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

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
                    int venueNum = 1;
                    while (reader.Read())
                    {
                        Venue venue = new Venue();
                        venue.Id = Convert.ToInt32(reader["id"]);
                        venue.Name = Convert.ToString(reader["name"]);
                        venue.CityId = Convert.ToInt32(reader["city_id"]);
                        venue.Description = Convert.ToString(reader["description"]);
                        venue.Address = Convert.ToString(reader["address"]);
                        venue.Categories = GetCategory(venue.Id);
                        venues[venueNum] = venue;
                        venueNum++;
                    };

                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying the database: " + ex.Message);
            }
            return venues;
        }

        public List<string> GetCategory(int venueId)
        {
            List<string> category = new List<string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                List<string> categories = new List<string>();

                SqlCommand command = new SqlCommand(SqlSelectCategoryName, conn);
                command.Parameters.AddWithValue("@id", venueId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    category.Add(Convert.ToString(reader["name"]));
                }
            }
            return category;
        }
    }
}
