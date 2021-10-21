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
            "SELECT id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy " +
            "FROM space s " +
            "INNER JOIN venue v ON v.id = s.venue_id" +
            "WHERE s.venue_id = @s.venue_id";

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
                    command.Parameters.AddWithValue("@s.venue_id", venueId);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Space space = new Space
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            VenueId = Convert.ToInt32(reader["venue_id"]),
                            Name = Convert.ToString(reader["name"]),
                            IsAccessible = Convert.ToBoolean(reader["is_accessible"]),
                            OpenDate = Convert.ToDateTime(reader["open_from"]),
                            CloseDate = Convert.ToDateTime(reader["open_to"]),
                            DailyRate = Convert.ToDecimal(reader["daily_rate"]),
                            MaxOccupancy = Convert.ToInt32(reader["max_occupancy"])
                        };

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
