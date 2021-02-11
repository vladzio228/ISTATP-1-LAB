using System;
using System.Collections.Generic;

#nullable disable

namespace GameLandWebApplication
{
    public partial class User
    {
        public User()
        {
            GamesUsers = new HashSet<GamesUser>();
        }

        public int UserId { get; set; }
        public string NickName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? NumberOfratedGames { get; set; }

        public virtual ICollection<GamesUser> GamesUsers { get; set; }
    }
}
