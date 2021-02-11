using System;
using System.Collections.Generic;

#nullable disable

namespace GameLandWebApplication
{
    public partial class GamesUser
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public byte Rate { get; set; }

        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
    }
}
