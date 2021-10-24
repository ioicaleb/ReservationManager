using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string ReservedBy { get; set; }
        public string VenueName { get; set; }
        public int SpaceId { get; set; }
        public string SpaceName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfAttendees { get; set; }
        public string TotalCost { get; set; }

        public Reservation GetReservationDetails(Space space, DateTime startDate, int stayLength, string reserver)
        {
            Reservation reservation = new Reservation
            {
                SpaceName = space.Name,
                VenueName = space.VenueName,
                SpaceId = space.Id,
                StartDate = startDate,
                EndDate = startDate.AddDays(stayLength),
                ReservedBy = reserver,
                TotalCost = space.TotalCost.ToString("C")
            };
            return reservation;
        }

        public override string ToString()
        {
            return String.Format("{0,-33}{1,-33}{2,-18}{3,-12}{4}",
            VenueName, SpaceName, ReservedBy.Replace(" Reservation", ""), StartDate.ToString("MM/dd/yyyy"), EndDate.ToString("MM/dd/yyyy"));
        }


    }
}
