using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        public List<int> GetAvailableSpaces(int venueId, DateTime startDate, int stayLength);

        public int ReserveSpace(Reservation reservation);

        public ICollection<Reservation> GetUpcomingReservations(int venueId);
        public Reservation SearchReservation(int reservationId);
    }
}
