using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicHub.Data.Models
{
    public class SongPerformer
    {
        [Required]
        public int SongId { get; set; }
        public Song Song { get; set; }
        public int PerformerId { get; set; }
        [Required]
        public Performer Performer { get; set; }
    }
}