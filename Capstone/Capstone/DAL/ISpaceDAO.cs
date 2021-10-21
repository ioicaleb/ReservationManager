<<<<<<< HEAD
﻿namespace Capstone
{
    internal interface ISpaceDAO
=======
﻿using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ISpaceDAO
>>>>>>> bfdb75e5f3bcee030e60f3e79e5fc48e1328d66d
    {
        public ICollection<Space> GetSpaces(int venueId);
    }
}