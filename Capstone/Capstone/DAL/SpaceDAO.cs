using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SpaceDAO : ISpaceDAO
    {
        private const string SqlSelectSpacesForVenue =
            "SELECT s.id, s.venue_id, s.name, s.is_accessible, ISNULL(s.open_from, 0) AS open_from, " +
                "ISNULL(s.open_to, 13) AS open_to, s.daily_rate, s.max_occupancy " +
            "FROM space s " +
                "INNER JOIN venue v ON v.id = s.venue_id " +
            "WHERE s.venue_id = @venue_id";

        private const string SqlSearch =
            "SELECT s.id, s.venue_id, s.name, s.is_accessible, ISNULL(s.open_from, 0) AS open_from, " +
                "ISNULL(s.open_to, 13) AS open_to, s.daily_rate, s.max_occupancy " +
            "FROM space s " +
                "INNER JOIN venue v ON v.id = s.venue_id " +
            "WHERE s.venue_id = @venue_id " +
                "AND max_occupancy >= @number_of_attendees " +
                "AND NOT EXISTS(SELECT * FROM reservation r " +
                "WHERE r.space_id = s.id AND start_date >= @start_date AND end_date <= @end_date)";

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
        public Dictionary<int, Space> GetSpaces(Venue venue)
        {
            Dictionary<int, Space> spaces = new Dictionary<int, Space>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectSpacesForVenue, conn);
                    command.Parameters.AddWithValue("@venue_id", venue.Id);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Space space = new Space
                        {
                            VenueId = venue.Id,
                            VenueName = venue.Name,
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            IsAccessible = Convert.ToBoolean(reader["is_accessible"]),
                            DailyRate = Convert.ToDecimal(reader["daily_rate"]),
                            MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]),
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

        /// <summary>
        /// Gathers each space obtained by the query as a value, setting each value = to a space which correlates with the key.
        /// The dictionary key is represented by the corresponding space ID.
        /// </summary>
        /// <param name="venue"></param>
        /// <param name="numberOfAttendees"></param>
        /// <param name="startDate"></param>
        /// <param name="stayLength"></param>
        /// <param name="category"></param>
        /// <param name="budget"></param>
        /// <returns></returns>
        public Dictionary<int, Space> SearchSpaces(Venue venue, int numberOfAttendees, DateTime startDate, int stayLength, string category, int budget)
        {
            Dictionary<int, Space> spaces = new Dictionary<int, Space>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    DateTime endDate = startDate.AddDays(stayLength);
                    SqlCommand command = new SqlCommand(SqlSearch, conn);
                    command.Parameters.AddWithValue("@venue_id", venue.Id);
                    command.Parameters.AddWithValue("@number_of_attendees", numberOfAttendees);
                    command.Parameters.AddWithValue("@start_date", startDate);
                    command.Parameters.AddWithValue("@end_date", endDate);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Space space = new Space
                        {
                            VenueName = venue.Name,
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            IsAccessible = Convert.ToBoolean(reader["is_accessible"]),
                            DailyRate = Convert.ToDecimal(reader["daily_rate"]),
                            OpenDate = Convert.ToInt32(reader["open_from"]),
                            CloseDate = Convert.ToInt32(reader["open_to"]),
                            MaxOccupancy = Convert.ToInt32(reader["max_occupancy"])
                        };

                        space.OpenMonth = ChangeIntToMonthAbbr(space.OpenDate);
                        space.CloseMonth = ChangeIntToMonthAbbr(space.CloseDate);

                        space.TotalCost = space.DailyRate * stayLength;

                        int desiredMonth = int.Parse(startDate.ToString("MM"));

                        if (space.TotalCost <= budget)
                        {
                            if (category == "None" || venue.Categories.Contains(category))
                            {
                                if(space.OpenDate <= desiredMonth && space.CloseDate >= desiredMonth)
                                {
                                    spaces[space.Id] = space;
                                }
                            }
                        }
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
