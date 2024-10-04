using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace SpelAvondApp.Data
{
    public class ApplicationUser : IdentityUser
    {

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

        public bool IsVolwassen
        {
            get
            {
                return (DateTime.Now.Year - Geboortedatum.Year) >= 18;
            }
        }
    }
}
