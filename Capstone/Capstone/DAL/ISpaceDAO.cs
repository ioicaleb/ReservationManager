using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// Interface implemented by SpaceDAO
    /// </summary>
    public interface ISpaceDAO
    {
        public Dictionary<int, Space> GetSpaces(Venue venue);
        public Dictionary<int, Space> SearchSpaces(Venue venue, int numberOfAttendees, DateTime startDate, int stayLength, string category, int budget, bool needAccessible);
    }
}