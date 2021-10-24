using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    ///Interface implemented by VenueDAO
    /// </summary>
    public interface IVenueDAO
    {
        public Dictionary<int, Venue> GetVenues();
    }
}
