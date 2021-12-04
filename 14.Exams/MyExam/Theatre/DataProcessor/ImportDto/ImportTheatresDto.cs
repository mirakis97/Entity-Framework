using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTheatresDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string Name { get; set; }
        [Required]
        [Range(1,10)]
        public int NumberOfHalls { get; set; }
        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string Director { get; set; }
        public ICollection<TicketDto> Tickets { get; set; }
        public class TicketDto
        {
            [Required]
            [Range(typeof(decimal), "1.00", "100.00")]
            public decimal Price { get; set; }
            [Required]
            [Range(1,10)]
            public int RowNumber { get; set; }
            [Required]
            public int PlayId { get; set; }
        }

    }
}