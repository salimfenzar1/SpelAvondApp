using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelAvondApp.Domain.Models
{
    public class BordspellenAvond
    {
        public int Id { get; set; } // Primary Key
        public string Adres { get; set; }
        public DateTime Datum { get; set; }
        public int MaxAantalSpelers { get; set; }
        public bool Is18Plus { get; set; } // Of de avond alleen voor 18+ is

        public string OrganisatorId { get; set; }
        public IdentityUser Organisator { get; set; }

        public ICollection<Inschrijving> Inschrijvingen { get; set; }

        public ICollection<Bordspel> Bordspellen { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
