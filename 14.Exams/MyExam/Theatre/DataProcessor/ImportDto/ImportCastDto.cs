using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Cast")]
    public class ImportCastDto
    {
        [XmlElement(ElementName = "FullName")]
        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string FullName { get; set; }

        [XmlElement(ElementName = "IsMainCharacter")]
        [Required]
        public bool IsMainCharacter { get; set; }

        [XmlElement(ElementName = "PhoneNumber")]
        [Required]
        [RegularExpression(@"^\+44-[0-9]{2}-[0-9]{3}-[0-9]{4}")]
        public string PhoneNumber { get; set; }

        [XmlElement(ElementName = "PlayId")]
        [Required]
        public int PlayId { get; set; }
    }
}
//•	Id – integer, Primary Key
//•	FullName – text with length [4, 30] (required)
//•	IsMainCharacter – Boolean represents if the actor plays the main character in a play (required)
//•	PhoneNumber - text in the following format: "+44-{2 numbers}-{3 numbers}-{4 numbers}".Valid phone numbers are: +44 - 53 - 468 - 3479, +44 - 91 - 842 - 6054, +44 - 59 - 742 - 3119(required)
//•	PlayId - integer, foreign key(required)