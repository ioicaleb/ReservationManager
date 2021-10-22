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
            "SELECT s.id, s.venue_id, s.name, s.is_accessible, ISNULL(s.open_from, 0) AS open_from, ISNULL(s.open_to, 0) AS open_to, s.daily_rate, s.max_occupancy, v.name AS venue_name " +
            "FROM space s " +
            "INNER JOIN venue v ON v.id = s.venue_id " +
            "WHERE s.venue_id = @venue_id";

        private readonly string connectionString;

        public SpaceDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ICollection<Space> GetSpaces(int venueId)
        {
            List<Space> spaces = new List<Space>();
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
                        int openDate = Convert.ToInt32(reader["open_from"]);
                        int closeDate = Convert.ToInt32(reader["open_to"]);
                        Space space = new Space
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            VenueId = Convert.ToInt32(reader["venue_id"]),
                            Name = Convert.ToString(reader["name"]),
                            IsAccessible = Convert.ToBoolean(reader["is_accessible"]),
                            DailyRate = Convert.ToDecimal(reader["daily_rate"]),
                            MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]),
                            VenueName = Convert.ToString(reader["venue_name"]),
                        };
                        if (!(openDate == 0))
                        {
                            space.OpenDate = openDate;
                        }
                        if (!(closeDate == 0))
                        {
                            space.CloseDate = closeDate;
                        }

                        spaces.Add(space);

                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying the database: " + ex.Message);
            }
            return spaces;
        }
    }
}
