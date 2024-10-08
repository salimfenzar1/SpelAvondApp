using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SpelAvondApp.Domain.Models
{
    public class BordspellenAvond
    {
        public int Id { get; set; } // Primary Key
        public string Adres { get; set; }
        public DateTime Datum { get; set; }
        public int MaxAantalSpelers { get; set; }
        public bool Is18Plus { get; set; } // Of de avond alleen voor 18+ is

        public string? OrganisatorId { get; set; }
        public IdentityUser? Organisator { get; set; }

        // Verzameling van meerdere bordspellen
        public ICollection<Bordspel> Bordspellen { get; set; } = new List<Bordspel>();

        public ICollection<Inschrijving> Inschrijvingen { get; set; } = new List<Inschrijving>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
