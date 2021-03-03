using System;
using System.Collections.Generic;

#nullable disable

namespace GameLandWebApplication
{
    public partial class Game
    {
        public Game()
        {
            GamesGenres = new HashSet<GamesGenre>();
            GamesPlatforms = new HashSet<GamesPlatform>();
            GamesSystemRequirements = new HashSet<GamesSystemRequirement>();
            GamesUsers = new HashSet<GamesUser>();
        }

        public int GameId { get; set; }
        public string GameName { get; set; }
        public int DeveloperId { get; set; }
        public int PublisherId { get; set; }
        public string LinkOnSteam { get; set; }
        public double? RatingByRedaction { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }

        public virtual Developer Developer { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<GamesGenre> GamesGenres { get; set; }
        public virtual ICollection<GamesPlatform> GamesPlatforms { get; set; }
        public virtual ICollection<GamesSystemRequirement> GamesSystemRequirements { get; set; }
        public virtual ICollection<GamesUser> GamesUsers { get; set; }
    }
}
