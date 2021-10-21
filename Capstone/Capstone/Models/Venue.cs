using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Venue
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    public override string ToString()
    {
        return $"{Name}\n" +
                $"Location: ";
    }

    }

}
