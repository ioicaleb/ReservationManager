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


        public override string ToString()
        {
            return String.Format("{0,0}{1,35}{2,22}{3,8}{4,11}",
            VenueName, SpaceName, ReservedBy, StartDate, EndDate);
        }


    }
}
