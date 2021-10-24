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
        public DateTime StartMonth { get; set; }
        public DateTime EndMonth { get; set; }
        public int NumberOfAttendees { get; set; }


        public override string ToString()
        {
            return String.Format("{0,-33}{1,-33}{2,-18}{3,-12}{4}",
            VenueName, SpaceName, ReservedBy.Replace(" Reservation", ""), StartDate.ToString("MM/dd/yyyy"), EndDate.ToString("MM/dd/yyyy"));
        }


    }
}
