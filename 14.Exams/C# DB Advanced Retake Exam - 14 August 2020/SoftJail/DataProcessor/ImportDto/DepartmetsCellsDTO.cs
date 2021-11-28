using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class DepartmetsCellsDTO
    {
        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }
        public List<Cell> Cells { get; set; }
        public class Cell
        {
            [Required]
            [Range(1 , 1000)]
            public int CellNumber { get; set; }
            [Required]
            public bool HasWindow { get; set; }
        }

    }
}
