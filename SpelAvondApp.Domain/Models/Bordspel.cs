using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpelAvondApp.Domain.Models
{
    public class Bordspel
    {
        public int Id { get; set; } // Primary Key

        [Required(ErrorMessage = "Naam is verplicht.")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Beschrijving is verplicht.")]
        public string Beschrijving { get; set; }

        [Required(ErrorMessage = "Genre is verplicht.")]
        public Genre Genre { get; set; }

        public bool Is18Plus { get; set; }

        [Required(ErrorMessage = "SoortSpel is verplicht.")]
        public SoortSpel SoortSpel { get; set; }

        public ICollection<BordspellenAvond> BordspellenAvonden { get; set; } = new List<BordspellenAvond>();
    }
}
