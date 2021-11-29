using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using VaporStore.Data.Models.Enums;

namespace VaporStore.DataProcessor.Dto.Import
{
    [XmlType("Purchase")]
    public class ImportPurchases
    {
        [XmlAttribute(AttributeName = "title")]
        public string Title { get; set; }
        [Required]
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }

        [XmlElement(ElementName = "Key")]
        [RegularExpression(@"([A-Z0-9]){4}-([A-Z0-9]){4}-([A-Z0-9]){4}")]
        [Required]
        public string Key { get; set; }

        [XmlElement(ElementName = "Card")]
        public string Card { get; set; }

        [XmlElement(ElementName = "Date")]
        public string Date { get; set; }

        //•	Id – integer, Primary Key
        //•	Type – enumeration of type PurchaseType, with possible values (“Retail”, “Digital”) (required)
        //•	ProductKey – text, which consists of 3 pairs of 4 uppercase Latin letters and digits, separated by dashes (ex. “ABCD-EFGH-1J3L”) (required)
        //•	Date – Date(required)
        //•	CardId – integer, foreign key(required)
        //•	Card – the purchase’s card (required)
        //•	GameId – integer, foreign key(required)
        //•	Game – the purchase’s game (required)

    }
}
