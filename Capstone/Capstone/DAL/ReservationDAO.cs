using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationDAO : IReservationDAO   
    {
        // Finds all of the spaces available to reserve, based on paramter filtering by user input, (when, how many days and people?).
        private const string SqlSelectAvailableSpaces =
            "SELECT s.id " +
            "FROM space s " +
            "WHERE s.venue_id = @venue_id " +
            "AND max_occupancy >= @number_of_attendees " +
            "AND NOT EXISTS(SELECT * FROM reservation r " +
            "WHERE r.space_id = s.id AND start_date >= @start_date AND end_date <= @end_date)";

        private const string SqlCreateReservation =
            "INSERT INTO reservation(space_id, number_of_attendees, start_date, end_date, reserved_for) " +
            "VALUES(@space_id, @number_of_attendees, @start_date, @end_date, @reserved_for) " +
            "SELECT @@IDENTITY";

        private const string SqlSelectNext30Days =
            "SELECT v.name, s.name, r.reserved_for, r.start_date, r.end_date " +
            "FROM reservation r " +
            "INNER JOIN space s ON s.id = r.space_id " +
            "INNER JOIN venue v ON v.id = s.venue_id " +
            "WHERE " +
            "v.id = @venueId AND start_date >= GETDATE() AND start_date <= DATEADD(day, 30, GETDATE()) " +
            "ORDER BY start_date ";

        private readonly string connectionString;

        public ReservationDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<int> GetAvailableSpaces(int venueId, DateTime startDate, int stayLength, int numberOfAttendees)
        {
            List<int> spacesAvailable = new List<int>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    DateTime endDate = startDate.AddDays(stayLength);
                    SqlCommand command = new SqlCommand(SqlSelectAvailableSpaces, conn);
                    command.Parameters.AddWithValue("@venue_id", venueId);
                    command.Parameters.AddWithValue("@number_of_attendees", numberOfAttendees);
                    command.Parameters.AddWithValue("@start_date", startDate);
                    command.Parameters.AddWithValue("@end_date", endDate);

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

        public int ReserveSpace(int space_id, int numberOfAttendees, DateTime startDate, int stayLength, string reservationName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    DateTime endDate = startDate.AddDays(stayLength);
                    SqlCommand command = new SqlCommand(SqlCreateReservation, conn);
                    command.Parameters.AddWithValue("@space_id", space_id);
                    command.Parameters.AddWithValue("@number_of_attendees", numberOfAttendees);
                    command.Parameters.AddWithValue("@start_date", startDate);
                    command.Parameters.AddWithValue("@end_date", endDate);
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

        public ICollection<Reservation> GetNext30Days(int venueId)
        {
            List<Reservation> reservations = new List<Reservation>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(SqlSelectNext30Days, conn);
                    command.Parameters.AddWithValue("@venueId", venueId);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation();

                        reservation.VenueName = Convert.ToString(reader["name"]);
                        reservation.SpaceName = Convert.ToString(reader["name"]);
                        reservation.ReservedBy = Convert.ToString(reader["reserved_for"]);
                        reservation.StartDate = Convert.ToDateTime(reader["start_date"]);
                        reservation.EndDate = Convert.ToDateTime(reader["end_date"]);

                        reservations.Add(reservation);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying database: " + ex.Message);
            }
            return reservations;
        }
    }
}
