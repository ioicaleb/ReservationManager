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
            "SELECT s.id, s.venue_id, s.name, s.is_accessible, s.open_from, s.open_to, s.daily_rate, s.max_occupancy, v.name " +
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
                            Id = Convert.ToInt32(reader["s.id"]),
                            VenueId = Convert.ToInt32(reader["s.venue_id"]),
                            Name = Convert.ToString(reader["s.name"]),
                            IsAccessible = Convert.ToBoolean(reader["s.is_accessible"]),
                            OpenDate = Convert.ToInt32(reader["s.open_from"]),
                            CloseDate = Convert.ToInt32(reader["s.open_to"]),
                            DailyRate = Convert.ToDecimal(reader["s.daily_rate"]),
                            MaxOccupancy = Convert.ToInt32(reader["s.max_occupancy"]),
                            VenueName = Convert.ToString(reader["v.name"])
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
