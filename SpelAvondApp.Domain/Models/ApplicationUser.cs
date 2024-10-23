using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace SpelAvondApp.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {

        [Required]
        public string ?Naam { get; set; }

        [Required]
        public string ?Geslacht { get; set; }

        [Required]
        public string ?Straat { get; set; }

        [Required]
        public string ?Huisnummer { get; set; }

        [Required]
        public string ?Stad { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? Geboortedatum { get; set; }

        public bool IsVolwassen
        {
            get
            {
                if (Geboortedatum.HasValue)
                {
                    var leeftijd = DateTime.Now.Year - Geboortedatum.Value.Year;
                    if (Geboortedatum.Value > DateTime.Now.AddYears(-leeftijd)) leeftijd--;
                    return leeftijd >= 18;
                }
                return false; 
            }
        }
        public bool HeeftLactoseAllergie { get; set; }
        public bool HeeftNotenAllergie { get; set; }
        public bool IsVegetarisch { get; set; }
        public bool GeenAlcohol { get; set; }
    }
}
