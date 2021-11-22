using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.DataProcessor.ImportDto
{
    public class AuthorsDto
    {
        [JsonProperty("FirstName")]
        [MinLength(3)]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MinLength(3)]
        [MaxLength(30)]
        [JsonProperty("LastName")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{3}-[0-90]{3}-[0-9]{4}$")]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public AuthorBookDto[] Books { get; set; }
    }
}
