using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        public List<int> GetReservations(int venueId, DateTime startDate, int stayLength, int numberOfAttendees);
    }
}
