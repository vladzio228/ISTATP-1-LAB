using System;
using System.Collections.Generic;

#nullable disable

namespace GameLandWebApplication
{
    public partial class SystemRequirement
    {
        public SystemRequirement()
        {
            GamesSystemRequirements = new HashSet<GamesSystemRequirement>();
        }

        public int SystemRequirementId { get; set; }
        public string SystemRequirementName { get; set; }

        public virtual ICollection<GamesSystemRequirement> GamesSystemRequirements { get; set; }
    }
}
