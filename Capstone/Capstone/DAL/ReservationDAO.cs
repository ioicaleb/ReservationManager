using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationDAO : IReservationDAO
    {
        private const string SqlSelectAvailableSpaces =
            "SELECT id " +
            "FROM space s " +
            "WHERE venue_id = @venue_id " +
            "AND (open_from <= @start_month AND open_to > @end_month " +
            "OR ISNULL(open_from, 0) = 0) " +
            "AND NOT EXISTS (SELECT * FROM reservation r " +
            "WHERE r.space_id = s.id AND r.start_date <= @end_date AND r.end_date >= @start_date)"; // This completely checks that a persons reservation can not be encapsulated in or overlappint, or be encapsulated by antoher persons existing reserevation.

        private const string SqlCreateReservation =
            "INSERT INTO reservation(space_id, start_date, end_date, reserved_for) " +
            "VALUES(@space_id, @start_date, @end_date, @reserved_for) " +
            "SELECT @@IDENTITY";

        private const string SqlSelectNext30Days =
            "SELECT v.name AS venue_name, s.name AS space_name, r.reserved_for, r.start_date, r.end_date " +
            "FROM reservation r " +
            "INNER JOIN space s ON s.id = r.space_id " +
            "INNER JOIN venue v ON v.id = s.venue_id " +
            "WHERE " +
            "v.id = @venueId AND start_date >= @start_date AND start_date <= @end_date " +
            "ORDER BY start_date ";

        private const string SqlSearchReservation =
           "SELECT r.*, s.name AS space_name, v.name AS venue_name " +
           "FROM reservation r " +
                "INNER JOIN space s ON s.id = r.space_id " +
                "INNER JOIN venue v ON v.id = s.venue_id " +
           "WHERE r.reservation_id = @reservation_id";

        private readonly string connectionString;

        public ReservationDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<int> GetAvailableSpaces(int venueId, DateTime startDate, int stayLength)
        {
            List<int> spacesAvailable = new List<int>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Calculating end date and obtaining months for both start and end dates, so they can be compared to the open_from and open_to month ints of the database
                    DateTime endDate = startDate.AddDays(stayLength);
                    int startMonth = int.Parse(startDate.ToString("MM"));
                    int endMonth = int.Parse(endDate.ToString("MM"));

                    SqlCommand command = new SqlCommand(SqlSelectAvailableSpaces, conn);

                    command.Parameters.AddWithValue("@venue_id", venueId);
                    command.Parameters.AddWithValue("@start_date", startDate);
                    command.Parameters.AddWithValue("@end_date", endDate);
                    command.Parameters.AddWithValue("@start_month", startMonth);
                    command.Parameters.AddWithValue("@end_month", endMonth);

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

        public int ReserveSpace(Reservation reservation)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlCreateReservation, conn);
                    command.Parameters.AddWithValue("@space_id", reservation.SpaceId);
                    command.Parameters.AddWithValue("@start_date", reservation.StartDate);
                    command.Parameters.AddWithValue("@end_date", reservation.EndDate);
                    command.Parameters.AddWithValue("@reserved_for", reservation.ReservedBy);

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

        public ICollection<Reservation> GetUpcomingReservations(int venueId)
        {
            List<Reservation> reservations = new List<Reservation>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectNext30Days, conn);
                    command.Parameters.AddWithValue("@venueId", venueId);
                    command.Parameters.AddWithValue("@start_date", DateTime.Today);
                    command.Parameters.AddWithValue("@end_date", DateTime.Today.AddDays(30));

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation();

                        reservation.VenueName = Convert.ToString(reader["venue_name"]);
                        reservation.SpaceName = Convert.ToString(reader["space_name"]);
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

        public Reservation SearchReservation(int reservationId)
        {
            Reservation reservation = new Reservation();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSearchReservation, conn);
                    command.Parameters.AddWithValue("@reservation_Id", reservationId);

                    SqlDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {

                        reservation.VenueName = Convert.ToString(reader["venue_name"]);
                        reservation.SpaceName = Convert.ToString(reader["space_name"]);
                        reservation.ReservedBy = Convert.ToString(reader["reserved_for"]);
                        reservation.StartDate = Convert.ToDateTime(reader["start_date"]);
                        reservation.EndDate = Convert.ToDateTime(reader["end_date"]);

                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying database: " + ex.Message);
            }
            return reservation;
        }
    }
}
