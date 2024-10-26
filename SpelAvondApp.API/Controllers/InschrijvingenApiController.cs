using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpelAvondApp.Application;
using SpelAvondApp.Domain.Models;
using System.Threading.Tasks;

namespace SpelAvondApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InschrijvingenApiController : ControllerBase
    {
        private readonly IInschrijvingService _inschrijvingService;
        private readonly UserManager<ApplicationUser> _userManager;

        public InschrijvingenApiController(IInschrijvingService inschrijvingService, UserManager<ApplicationUser> userManager)
        {
            _inschrijvingService = inschrijvingService;
            _userManager = userManager;
        }

        [HttpPost("inschrijven/{avondId}")]
        public async Task<IActionResult> Inschrijven(int avondId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Gebruiker niet gevonden.");
            }

            var inschrijvingBestaatAl = await _inschrijvingService.HeeftAlIngeschreven(user.Id, avondId);
            if (inschrijvingBestaatAl)
            {
                return BadRequest("Je bent al ingeschreven voor deze avond.");
            }

            var avond = await _inschrijvingService.GetAvondMetDieetOptiesAsync(avondId);
            if (avond == null)
            {
                return NotFound("Bordspellenavond niet gevonden.");
            }

            var heeftInschrijvingOpDatum = await _inschrijvingService.KanDeelnemenAanAvond(user, avond.Datum);
            if (heeftInschrijvingOpDatum)
            {
                return BadRequest("Je kunt slechts aan één avond per dag deelnemen.");
            }

            var magDeelnemen = await _inschrijvingService.MagDeelnemenOpBasisVanLeeftijdAsync(user.Id, avondId);
            if (!magDeelnemen)
            {
                return BadRequest("Niet oud genoeg voor deze 18+ avond.");
            }

            if ((user.HeeftLactoseAllergie && !avond.BiedtLactosevrijeOpties) ||
                (user.HeeftNotenAllergie && !avond.BiedtNotenvrijeOpties) ||
                (user.IsVegetarisch && !avond.BiedtVegetarischeOpties) ||
                (user.GeenAlcohol && !avond.BiedtAlcoholvrijeOpties))
            {
                return BadRequest("De avond voldoet mogelijk niet aan je dieetwensen.");
            }

            var success = await _inschrijvingService.InschrijvenVoorAvondAsync(user.Id, avondId, "Geen specifieke dieetwensen");
            if (success)
            {
                return Ok("Inschrijving succesvol.");
            }
            else
            {
                return BadRequest("Inschrijving mislukt. Mogelijk is het maximum aantal spelers bereikt.");
            }
        }
    }
}
