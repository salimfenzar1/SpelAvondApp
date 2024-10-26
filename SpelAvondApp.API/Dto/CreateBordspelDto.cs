using SpelAvondApp.Domain.Models;
using System.ComponentModel.DataAnnotations;

public class CreateBordspelDto
{
    [Required(ErrorMessage = "Naam is verplicht.")]
    public string Naam { get; set; } = string.Empty;

    [Required(ErrorMessage = "Beschrijving is verplicht.")]
    public string Beschrijving { get; set; } = string.Empty;

    [Required(ErrorMessage = "Genre is verplicht.")]
    public Genre Genre { get; set; }

    public bool Is18Plus { get; set; }

    [Required(ErrorMessage = "SoortSpel is verplicht.")]
    public SoortSpel SoortSpel { get; set; }

    public string FotoPath { get; set; } = string.Empty;
}