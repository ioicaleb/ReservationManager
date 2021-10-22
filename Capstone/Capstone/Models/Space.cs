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
        public string Accessible 
        { 
            get 
            { 
                if (IsAccessible) 
                { 
                    return "Yes"; 
                } 
                return "No"; 
            } 
        }
        public int OpenDate { get; set; }
        public int CloseDate { get; set; }
        public string OpenMonth { get; set; }
        public string CloseMonth { get; set; }
        public decimal DailyRate { get; set; }
        public int MaxOccupancy { get; set; }
        public string VenueName { get; set; }
        public decimal TotalCost { get; set; }

        public override string ToString()
        {
            return $"{Id} {Name} {DailyRate} {MaxOccupancy} {Accessible} {TotalCost}";
        }
    }
}
