using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class handles working with Venues in the database.
    /// </summary>
    public class VenueDAO
    {
        private readonly string connectionString;

        public VenueDAO (string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
