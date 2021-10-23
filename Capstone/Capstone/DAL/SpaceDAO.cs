using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SpaceDAO : ISpaceDAO
    {
        private const string SqlSelect =
            "SELECT s.id, s.venue_id, s.name, s.is_accessible, ISNULL(s.open_from, 0) AS open_from, ISNULL(s.open_to, 13) AS open_to, s.daily_rate, s.max_occupancy, v.name AS venue_name " +
            "FROM space s " +
            "INNER JOIN venue v ON v.id = s.venue_id " +
            "WHERE s.venue_id = @venue_id";

        private readonly string connectionString;

        public SpaceDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// A new dictionary of Key: Space ID and Value: Space, created by obtaining all spaces from a selected venue.
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        public Dictionary<int,Space> GetSpaces(int venueId)
        {
            Dictionary<int, Space> spaces = new Dictionary<int, Space>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelect, conn);
                    command.Parameters.AddWithValue("@venue_id", venueId);

                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        Space space = new Space
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            VenueId = Convert.ToInt32(reader["venue_id"]),
                            Name = Convert.ToString(reader["name"]),
                            IsAccessible = Convert.ToBoolean(reader["is_accessible"]),
                            DailyRate = Convert.ToDecimal(reader["daily_rate"]),
                            MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]),
                            VenueName = Convert.ToString(reader["venue_name"]),
                            OpenDate = Convert.ToInt32(reader["open_from"]),
                            CloseDate = Convert.ToInt32(reader["open_to"])
                        };

                        space.OpenMonth = ChangeIntToMonthAbbr(space.OpenDate);
                        space.CloseMonth = ChangeIntToMonthAbbr(space.CloseDate);

                        // Collects the actual id from the space as a key, and the entire space will be stored as it's value.
                        spaces[space.Id] = space;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying the database: " + ex.Message);
            }
            return spaces;
        }
        public string ChangeIntToMonthAbbr(int date)
        {
            List<string> months = new List<string> { "ALWAYS", "Jan.", "Feb.", "Mar.", "Apr.", "May", "Jun.", "Jul.", "Aug.", "Sep.", "Oct.", "Nov.", "Dec.", "NEVER" };
            return months[date];
        }
    }
}
