using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationDAO : IReservationDAO
    {
        private const string SqlSelect =
            "SELECT s.id " +
            "FROM space s " +
            "WHERE s.venue_id = @venue_id " +
            "AND max_occupancy < @number_of_attendees " +
            "AND NOT EXISTS(SELECT * FROM reservation r " +
            "WHERE r.space_id = s.id AND start_date >= @start_date AND end_date <= @end_date)";

        private const string SqlCreateReservation =
            "INSERT INTO reservation(space_id, number_of_attendees, start_date, end_date, reserved_for) " +
            "VALUES(@space_id, @number_of_attendees, @start_date, DATEADD(day, @stay_length, @start_date), @reserved_for) " +
            "SELECT @@IDENTITY";

        private readonly string connectionString;

        public ReservationDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<int> GetReservations(int venueId, DateTime startDate, int stayLength, int numberOfAttendees)
        {
            List<int> spacesAvailable = new List<int>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    DateTime endDate = startDate.AddDays(stayLength);
                    SqlCommand command = new SqlCommand(SqlSelect, conn);
                    command.Parameters.AddWithValue("@venue_id", venueId);
                    command.Parameters.AddWithValue("@number_of_attendees", numberOfAttendees);
                    command.Parameters.AddWithValue("@start_date", startDate);
                    command.Parameters.AddWithValue("@start_date", endDate);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        spacesAvailable.Add(Convert.ToInt32(reader["id"]));

                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying the database: " + ex.Message);
            }
            return spacesAvailable;
        }

        public int ReserveSpace(int space_id, int numberOfAttendees, string startDate, int stayLength, string reservationName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlCreateReservation, conn);
                    command.Parameters.AddWithValue("@space_id", space_id);
                    command.Parameters.AddWithValue("@number_of_attendees", numberOfAttendees);
                    command.Parameters.AddWithValue("@start_date", Convert.ToDateTime(startDate));
                    command.Parameters.AddWithValue("@stay_length", stayLength);
                    command.Parameters.AddWithValue("@reserved_for", reservationName);

                    int id = Convert.ToInt32(command.ExecuteScalar());

                    return id;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying database: " + ex.Message);
            }

            return -1;
        }
    }
}
