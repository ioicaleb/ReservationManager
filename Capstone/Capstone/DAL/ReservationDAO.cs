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
            "SELECT r.id, r.space_id, r.number_of_attendees, r.start_date, r.end_date, r.reserved_for, s.name" +
            "FROM reservation r INNER JOIN space s ON s.id = r.space_id " +
            "WHERE r.space_id = @space_id";

        private const string SqlCreateReservation =
            "INSERT INTO reservation(space_id, number_of_attendees, start_date, end_date, reserved_for) " +
            "VALUES(@space_id, @number_of_attendees, @start_date, DATEADD(day, @stay_length, @start_date), @reserved_for) " +
            "SELECT @@IDENTITY";

        private readonly string connectionString;

        public ReservationDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ICollection<Reservation> GetReservations(int spaceId)
        {
            List<Reservation> reservations = new List<Reservation>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelect, conn);
                    command.Parameters.AddWithValue("@space_id", spaceId);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation
                        {
                            Id = Convert.ToInt32(reader["r.id"]),
                            SpaceId = Convert.ToInt32(reader["r.space_id"]),
                            NumberOfAttendees = Convert.ToInt32(reader["r.number_of_attendees"]),
                            StartDate = Convert.ToDateTime(reader["r.start_date"]),
                            EndDate = Convert.ToDateTime(reader["r.end_date"]),
                            ReservedBy = Convert.ToString(reader["r.reserved_for"]),
                            SpaceName = Convert.ToString(reader["s.name"])
                        };

                        reservations.Add(reservation);

                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying the database: " + ex.Message);
            }
            return reservations;
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
