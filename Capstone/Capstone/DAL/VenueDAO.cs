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
        //<<<<<<< HEAD
        private const string SqlSelectVenues =
            "SELECT v.id, v.name, v.city_id, v.description, c.name + ', ' + c.state_abbreviation AS address " +
            "FROM venue v INNER JOIN city c ON c.id = v.city_id ";
            //"ORDER BY v.name";

//=======
        //private const string SqlSelect =
        //    "SELECT v.id, v.name, v.city_id, v.description, c.name + ', ' + c.state_abbreviation AS address, c.name AS categoryName " +
        //    "FROM venue v " +
        //    "INNER JOIN city c ON c.id = v.city_id";
//>>>>>>> bf9b38ab2d138f795b46dca3d58df9f23d55ecf9

        private const string SqlSelectCategoryName =
            "SELECT c.name " +
            "FROM venue v INNER JOIN category_venue cv ON cv.venue_id = v.id INNER JOIN category c ON c.id = cv.category_id " +
            "WHERE v.id = @id";

        private readonly string connectionString;

        public VenueDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ICollection<Venue> GetVenues()
        {
            List<Venue> venues = new List<Venue>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectVenues, conn);

                    SqlDataReader reader = command.ExecuteReader();
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
                        venues.Add(venue);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying the database: " + ex.Message);
            }

            foreach (Venue venue in venues)
            {
                venue.Categories = GetCategory(venue.Id);
            }

            return venues;
        }

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
