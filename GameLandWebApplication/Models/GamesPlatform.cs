using System;
using System.Collections.Generic;

#nullable disable

namespace GameLandWebApplication
{
    public partial class GamesPlatform
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int PlatformId { get; set; }
        public DateTime ReleaseDate { get; set; }

        public virtual Game Game { get; set; }
        public virtual Platform Platform { get; set; }
    }
}
