using System;
using System.Collections.Generic;

#nullable disable

namespace GameLandWebApplication
{
    public partial class GamesGenre
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int GenreId { get; set; }

        public virtual Game Game { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
