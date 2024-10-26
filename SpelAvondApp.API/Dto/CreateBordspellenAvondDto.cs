using System.ComponentModel.DataAnnotations;

public class CreateBordspellenAvondDto
{
    [Required(ErrorMessage = "Het adres is verplicht.")]
    public string Adres { get; set; }

    [Required(ErrorMessage = "De datum is verplicht.")]
    public DateTime Datum { get; set; }

    [Required(ErrorMessage = "Het maximale aantal spelers is verplicht.")]
    public int MaxAantalSpelers { get; set; }

    public bool Is18Plus { get; set; }
    public bool BiedtLactosevrijeOpties { get; set; }
    public bool BiedtNotenvrijeOpties { get; set; }
    public bool BiedtVegetarischeOpties { get; set; }
    public bool BiedtAlcoholvrijeOpties { get; set; }
    public List<int> BordspelIds { get; set; }
}
