using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationDAO : IReservationDAO
    {
        private readonly string connectionString;

        public ReservationDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
