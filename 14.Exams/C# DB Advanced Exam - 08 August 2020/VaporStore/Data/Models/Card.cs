using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Card
    {
        public Card()
        {
            this.Purchases = new HashSet<Purchase>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Cvc { get; set; }
        [Required]
        public CardType Type { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    }
}

