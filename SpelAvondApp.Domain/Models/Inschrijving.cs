using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelAvondApp.Domain.Models
{
    public class Inschrijving
    {
        public int Id { get; set; }

        public string SpelerId { get; set; }
        public string DieetWensen { get; set; }

        [NotMapped]
        public IdentityUser Speler { get; set; }

        // Buitenlandse sleutel naar de BordspellenAvond
        public int BordspellenAvondId { get; set; }
        public BordspellenAvond BordspellenAvond { get; set; }

        public bool Aanwezig { get; set; }
    }
}
