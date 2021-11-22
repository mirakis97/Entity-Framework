﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType("Books")]
    public class BooksDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Range(1, 3)]
        [XmlElement("Genre")]
        public int Genre { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [XmlElement("Price")]
        public decimal Price { get; set; }

        [Range(50, 5000)]
        [XmlElement("Pages")]
        public int Pages { get; set; }


        [XmlElement("PublishedOn")]
        public string PublishedOn { get; set; }
    }
}
    //< Name > Hairy Torchwood </ Name >
   
    //   < Genre > 3 </ Genre >
   
    //   < Price > 41.99 </ Price >
   
    //   < Pages > 3013 </ Pages >
   
    //   < PublishedOn > 01 / 13 / 2013 </ PublishedOn >