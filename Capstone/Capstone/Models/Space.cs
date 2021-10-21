using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Space
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VenueId { get; set; }
        public bool IsAccessible { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public decimal DailyRate { get; set; }
        public int MaxOccupancy { get; set; }
    }
}
