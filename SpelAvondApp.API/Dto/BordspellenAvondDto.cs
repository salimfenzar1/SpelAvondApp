public class BordspellenAvondDto
{
    public int Id { get; set; }
    public string Adres { get; set; }
    public DateTime Datum { get; set; }
    public int MaxAantalSpelers { get; set; }
    public bool Is18Plus { get; set; }
    public string OrganisatorUsername { get; set; }
    public List<string> InschrijvingenUsernames { get; set; }
    public List<string> BordspelNamen { get; set; }
    public double GemiddeldeScore { get; set; }
}
