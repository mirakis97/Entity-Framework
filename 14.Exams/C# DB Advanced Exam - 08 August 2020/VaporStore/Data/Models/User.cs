﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.Data.Models
{
    public class User
    {
        public User()
        {
            this.Cards = new HashSet<Card>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}
