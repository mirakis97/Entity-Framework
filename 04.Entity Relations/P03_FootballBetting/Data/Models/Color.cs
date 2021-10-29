using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class Color
    {
        public Color()
        {
            this.PrimaryKitTeams = new HashSet<Team>();
            this.SecondaryKitTeams = new HashSet<Team>();
        }
        [Key]
        public int ColorId { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public ICollection<Team> PrimaryKitTeams { get; set; }
        [NotMapped]
        public ICollection<Team> SecondaryKitTeams { get; set; }

    }
}
