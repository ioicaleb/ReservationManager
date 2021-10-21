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
    }
}
