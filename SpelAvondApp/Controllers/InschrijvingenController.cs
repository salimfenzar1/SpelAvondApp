using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpelAvondApp.Application;
using SpelAvondApp.Domain.Models;

namespace SpelAvondApp.Controllers
{
    [Authorize]
    public class InschrijvingenController : Controller
    {
        private readonly IInschrijvingService _inschrijvingService;
        private readonly UserManager<ApplicationUser> _userManager;

        public InschrijvingenController(IInschrijvingService inschrijvingService, UserManager<ApplicationUser> userManager)
        {
            _inschrijvingService = inschrijvingService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Inschrijven(int avondId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Er is geen geregistreerde gebruiker gevonden.";
                return RedirectToAction("Index", "BordspellenAvond");
            }

            // Controleer of de gebruiker al is ingeschreven
            var inschrijvingBestaatAl = await _inschrijvingService.HeeftAlIngeschreven(user.Id, avondId);
            if (inschrijvingBestaatAl)
            {
                TempData["ErrorMessage"] = "Je bent al ingeschreven voor deze bordspellenavond.";
                return RedirectToAction("Index", "BordspellenAvond");
            }

            // Haal de bordspellenavond inclusief dieetopties op
            var avond = await _inschrijvingService.GetAvondMetDieetOptiesAsync(avondId);
            if (avond == null)
            {
                TempData["ErrorMessage"] = "De bordspellenavond is niet gevonden.";
                return RedirectToAction("Index", "BordspellenAvond");
            }

            var heeftInschrijvingOpDatum = await _inschrijvingService.KanDeelnemenAanAvond(user, avond.Datum);
            if (heeftInschrijvingOpDatum)
            {
                TempData["ErrorMessage"] = "Je kunt slechts aan één bordspellenavond per dag deelnemen.";
                return RedirectToAction("Index", "BordspellenAvond");
            }

            var magDeelnemen = await _inschrijvingService.MagDeelnemenOpBasisVanLeeftijdAsync(user.Id, avondId);
            if (!magDeelnemen)
            {
                TempData["ErrorMessage"] = "Je bent niet oud genoeg om deel te nemen aan deze 18+ bordspellenavond.";
                return RedirectToAction("Index", "BordspellenAvond");
            }

            if ((user.HeeftLactoseAllergie && !avond.BiedtLactosevrijeOpties) ||
                (user.HeeftNotenAllergie && !avond.BiedtNotenvrijeOpties) ||
                (user.IsVegetarisch && !avond.BiedtVegetarischeOpties) ||
                (user.GeenAlcohol && !avond.BiedtAlcoholvrijeOpties))
            {
                TempData["ErrorMessage"] = "Let op: De bordspellenavond voldoet mogelijk niet aan jouw dieetwensen of allergieën.";
                return RedirectToAction("Index", "BordspellenAvond");
            }

            // Schrijf de gebruiker in voor de avond
            var success = await _inschrijvingService.InschrijvenVoorAvondAsync(user.Id, avondId, "Geen specifieke dieetwensen");
            if (success)
            {
                TempData["SuccessMessage"] = "Je bent succesvol ingeschreven voor de bordspellenavond!";
            }
            else
            {
                TempData["ErrorMessage"] = "Inschrijving mislukt. Mogelijk is het maximale aantal spelers al bereikt.";
            }

            return RedirectToAction("Index", "BordspellenAvond");
        }

    }
}
