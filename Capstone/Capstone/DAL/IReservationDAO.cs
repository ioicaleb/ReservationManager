using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        public ICollection<Reservation> GetReservations(int spaceId);
    }
}
