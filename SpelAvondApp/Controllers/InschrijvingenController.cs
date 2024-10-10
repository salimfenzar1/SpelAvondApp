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

            var inschrijvingBestaatAl = await _inschrijvingService.HeeftAlIngeschreven(user.Id, avondId);
            if (inschrijvingBestaatAl)
            {
                TempData["ErrorMessage"] = "Je bent al ingeschreven voor deze bordspellenavond.";
                return RedirectToAction("Index", "BordspellenAvond");
            }

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
