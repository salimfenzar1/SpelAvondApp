using SpelAvondApp.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpelAvondApp.Domain.Models
{
    public class BordspellenAvond
    {
        [Required(ErrorMessage = "De datum is verplicht.")]
        public int Id { get; set; }
        public string ?Adres { get; set; }
        [Required(ErrorMessage = "Het adres is verplicht.")]
        public DateTime Datum { get; set; }
        [Required(ErrorMessage = "Het maximale aantal spelers is verplicht.")]
        public int MaxAantalSpelers { get; set; }
        public bool Is18Plus { get; set; }

        public string? OrganisatorId { get; set; }
        [NotMapped]
        public ApplicationUser? Organisator { get; set; }
        public bool BiedtLactosevrijeOpties { get; set; }
        public bool BiedtNotenvrijeOpties { get; set; }
        public bool BiedtVegetarischeOpties { get; set; }
        public bool BiedtAlcoholvrijeOpties { get; set; }

        public ICollection<Bordspel> Bordspellen { get; set; } = new List<Bordspel>();
        public ICollection<Inschrijving> Inschrijvingen { get; set; } = new List<Inschrijving>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        [NotMapped]
        public double GemiddeldeScore { get; set; } 
    
}
}
