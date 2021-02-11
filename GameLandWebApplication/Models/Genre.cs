using System;
using System.Collections.Generic;

#nullable disable

namespace GameLandWebApplication
{
    public partial class Genre
    {
        public Genre()
        {
            GamesGenres = new HashSet<GamesGenre>();
        }

        public int GenreId { get; set; }
        public string GenreName { get; set; }

        public virtual ICollection<GamesGenre> GamesGenres { get; set; }
    }
}
