using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class ImportPlaysDto
    {
        //•	Id – integer, Primary Key
        //•	Title – text with length [4, 50] (required)
        //•	Duration – TimeSpan in format
        //{ hours: minutes: seconds}, with a minimum length of 1 hour. (required)
        //•	Rating – float in the range[0.00….10.00] (required)
        //•	Genre – enumeration of type Genre, with possible values (Drama, Comedy, Romance, Musical) (required)
        //•	Description – text with length up to 700 characters (required)
        //•	Screenwriter – text with length [4, 30] (required)
        //•	Casts - a collection of type Cast
        //•	Tickets - a collection of type Ticket
        [XmlElement(ElementName = "Title")]
        [Required]
        [MaxLength(50)]
        [MinLength(4)]
        public string Title { get; set; }

        [XmlElement(ElementName = "Duration")]
        [Required]
        public string Duration { get; set; }

        [XmlElement(ElementName = "Rating")]
        [Range(0.00,10.00)]
        public float Rating { get; set; }

        [XmlElement(ElementName = "Genre")]
        [Required]
        public string Genre { get; set; }

        [XmlElement(ElementName = "Description")]
        [MaxLength(700)]
        [Required]
        public string Description { get; set; }

        [XmlElement(ElementName = "Screenwriter")]
        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string Screenwriter { get; set; }
    }
}
