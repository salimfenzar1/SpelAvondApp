using System;
using System.ComponentModel.DataAnnotations;

namespace SpelAvondApp.API.Models
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Naam { get; set; }

        [Required]
        public string Geslacht { get; set; }

        [Required]
        public string Straat { get; set; }

        [Required]
        public string Huisnummer { get; set; }

        [Required]
        public string Stad { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Geboortedatum { get; set; }

        public bool HeeftLactoseAllergie { get; set; }
        public bool HeeftNotenAllergie { get; set; }
        public bool IsVegetarisch { get; set; }
        public bool GeenAlcohol { get; set; }
    }
}
