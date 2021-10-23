using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        // should this be a spaces dao method
        public List<int> GetAvailableSpaces(int venueId, DateTime startDate, int stayLength, int numberOfAttendees);

        public int ReserveSpace(int space_id, int numberOfAttendees, DateTime startDate, int stayLength, string reservationName);

        public ICollection<Reservation> GetNext30Days(int venueId);
    }
}
