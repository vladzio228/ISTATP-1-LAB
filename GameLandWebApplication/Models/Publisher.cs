using System;
using System.Collections.Generic;

#nullable disable

namespace GameLandWebApplication
{
    public partial class Publisher
    {
        public Publisher()
        {
            Games = new HashSet<Game>();
        }

        public int PublisherId { get; set; }
        public string PublisherName { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
