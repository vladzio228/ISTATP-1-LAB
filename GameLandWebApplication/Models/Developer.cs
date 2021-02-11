using System;
using System.Collections.Generic;

#nullable disable

namespace GameLandWebApplication
{
    public partial class Developer
    {
        public Developer()
        {
            Games = new HashSet<Game>();
        }

        public int DeveloperId { get; set; }
        public string DeveloperName { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
