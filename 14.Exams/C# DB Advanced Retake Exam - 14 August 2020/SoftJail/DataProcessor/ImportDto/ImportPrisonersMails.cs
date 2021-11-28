using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonersMails
    {
        [Required]
        [StringLength(20,MinimumLength = 3)]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(@"The [A-Z]{1}[a-z]*")]
        public string Nickname { get; set; }
        [Required]
        [Range(18,65)]
        public int Age { get; set; }
        [Required]
        public string IncarcerationDate { get; set; }
        public string? ReleaseDate { get; set; }
        public decimal? Bail { get; set; }
        public int CellId { get; set; }
        public List<Mail> Mails { get; set; }

        public class Mail
        {
            [Required]
            public string Description { get; set; }
            [Required]
            public string Sender { get; set; }
            [Required]
            [RegularExpression(@"^([A-z0-9\s]+str.)$")]
            public string Address { get; set; }
        }

    }
}
