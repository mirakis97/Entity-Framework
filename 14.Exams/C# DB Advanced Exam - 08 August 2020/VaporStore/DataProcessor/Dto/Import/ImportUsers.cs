using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VaporStore.Data.Models.Enums;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class ImportUsers
    {
       
        [Required]
        [RegularExpression(@"([A-z]+ [A-z])\w+")]
        public string FullName { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Range(3,103)]
        public int Age { get; set; }
        public List<Card> Cards { get; set; }
        public class Card
        {
            [Required]
            [RegularExpression(@"[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}")]
            public string Number { get; set; }
            [Required]
            [RegularExpression(@"[0-9]{3}")]
            public string CVC { get; set; }
            [Required]
            [EnumDataType(typeof(CardType))]
            public string Type { get; set; }
        }

        //•	Id – integer, Primary Key
        //•	Number – text, which consists of 4 pairs of 4 digits, separated by spaces (ex. “1234 5678 9012 3456”) (required)
        //•	Cvc – text, which consists of 3 digits (ex. “123”) (required)
        //•	Type – enumeration of type CardType, with possible values (“Debit”, “Credit”) (required)
        //•	UserId – integer, foreign key(required)
        //•	User – the card’s user (required)
        //•	Purchases – collection of type Purchase
    }
}
