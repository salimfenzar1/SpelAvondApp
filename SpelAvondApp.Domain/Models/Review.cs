using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelAvondApp.Domain.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int BordspellenAvondId { get; set; }
        public BordspellenAvond BordspellenAvond { get; set; }

        public string SpelerId { get; set; }
        public IdentityUser Speler { get; set; }

        public int Score { get; set; } 
        public string Opmerking { get; set; } 
    }
}
