using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    /// <summary>
    /// This class represents a single space, it has properties that will be set and pulled as well as specific format of being printed out.
    /// </summary>
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
        public DateTime UpcomingReservation { get; set; }

        // The default format for printing a space
        public override string ToString()
        {
            return String.Format("{0,-10}{1,-33}{2,-13}{3,-12}{4,-13}{5}",
            Id, Name, DailyRate.ToString("C"), MaxOccupancy, Accessible, TotalCost.ToString("C"));
        }
    }
}
