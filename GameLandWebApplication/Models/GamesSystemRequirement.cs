using System;
using System.Collections.Generic;

#nullable disable

namespace GameLandWebApplication
{
    public partial class GamesSystemRequirement
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int SystemRequirementId { get; set; }
        public string Os { get; set; }
        public string Cpu { get; set; }
        public string Videocard { get; set; }
        public string Ram { get; set; }
        public string SpaceOnStorage { get; set; }
        public string DirectX { get; set; }

        public virtual Game Game { get; set; }
        public virtual SystemRequirement SystemRequirement { get; set; }
    }
}
