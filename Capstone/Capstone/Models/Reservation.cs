using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string ReservedBy { get; set; }
        public int SpaceId { get; set; }
        public string SpaceName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfAttendees { get; set; }
    }
}
