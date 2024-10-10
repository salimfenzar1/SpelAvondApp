using SpelAvondApp.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpelAvondApp.Domain.Models
{
    public class BordspellenAvond
    {
        public int Id { get; set; }
        public string Adres { get; set; }
        public DateTime Datum { get; set; }
        public int MaxAantalSpelers { get; set; }
        public bool Is18Plus { get; set; }

        // Opslaan van de UserId als string
        public string? OrganisatorId { get; set; }

        // Navigatie-eigenschap zonder fysieke koppeling in de database
        [NotMapped]
        public ApplicationUser? Organisator { get; set; }

        public ICollection<Bordspel> Bordspellen { get; set; } = new List<Bordspel>();
        public ICollection<Inschrijving> Inschrijvingen { get; set; } = new List<Inschrijving>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
