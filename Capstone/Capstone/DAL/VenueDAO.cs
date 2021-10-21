﻿using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class handles working with Venues in the database.
    /// </summary>
    public class VenueDAO : IVenueDAO
    {
        private const string SqlSelect =
            "SELECT v.id, v.name, v.city_id,	v.description, c.name + ', ' + c.state_abbreviation AS Address " +
            "FROM venue v INNER JOIN city c ON c.id = v.city_id";

        private readonly string connectionString;

        public VenueDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ICollection<Venue> GetVenues()
        {
            List<Venue> venues = new List<Venue>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelect, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Venue venue = new Venue
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            CityId = Convert.ToInt32(reader["city_id"]),
                            Description = Convert.ToString(reader["description"]),
                            Address = Convert.ToString(reader["Address"])
                        };

                        venues.Add(venue);

                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem querying the database: " + ex.Message);
            }
            return venues;
        }
    }
}
