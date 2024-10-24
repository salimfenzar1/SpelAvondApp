using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using SpelAvondApp.Application;
using SpelAvondApp.Domain.Models;

[Authorize]
public class BordspellenAvondController : Controller
{
    private readonly IBordspellenAvondService _bordspellenAvondService;
    private readonly IInschrijvingService _inschrijvingService;
    private readonly UserManager<ApplicationUser> _userManager;

    public BordspellenAvondController(IBordspellenAvondService bordspellenAvondService, UserManager<ApplicationUser> userManager, IInschrijvingService inschrijvingService)
    {
        _bordspellenAvondService = bordspellenAvondService;
        _userManager = userManager;
        _inschrijvingService = inschrijvingService;
    }

    public async Task<IActionResult> Index()
    {
        var avonden = await _bordspellenAvondService.GetAllAvondenAsync();

        foreach (var avond in avonden)
        {
            if (!string.IsNullOrEmpty(avond.OrganisatorId))
            {
                var organisator = await _userManager.FindByIdAsync(avond.OrganisatorId);
                avond.Organisator = organisator;
                avond.GemiddeldeScore = await _bordspellenAvondService.BerekenGemiddeldeScoreOrganisatorAsync(avond.OrganisatorId);
            }
        }

        return View(avonden);
    }

    public async Task<IActionResult> Create()
    {
        var bordspellen = await _bordspellenAvondService.GetAllBordspellenAsync();
        ViewBag.Bordspellen = new MultiSelectList(bordspellen, "Id", "Naam");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(BordspellenAvond model, List<int> geselecteerdeBordspellen)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            TempData["ErrorMessage"] = "Er is geen geregistreerde gebruiker gevonden.";
            return RedirectToAction(nameof(Create));
        }

        if (!await _bordspellenAvondService.IsUserEligibleToOrganizeAsync(user))
        {
            TempData["ErrorMessage"] = "Je moet minimaal 18 jaar oud zijn om een bordspellenavond te organiseren.";
            return RedirectToAction(nameof(Create));
        }

        var isValid = await _bordspellenAvondService.ValidateBordspellenAvond(model, geselecteerdeBordspellen);
        if (!isValid)
        {
            TempData["ErrorMessage"] = "Je kunt geen niet-18+ avond aanmaken met een 18+ spel.";
            return RedirectToAction(nameof(Create));
        }

        model.OrganisatorId = user.Id;

        if (ModelState.IsValid)
        {
            await _bordspellenAvondService.CreateBordspellenAvondAsync(model, geselecteerdeBordspellen);
            TempData["SuccessMessage"] = "De bordspellenavond is succesvol aangemaakt.";
            return RedirectToAction(nameof(Index));
        }

        var bordspellen = await _bordspellenAvondService.GetAllBordspellenAsync();
        ViewBag.Bordspellen = new MultiSelectList(bordspellen, "Id", "Naam");
        return View(model);
    }


    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var avond = await _bordspellenAvondService.GetAvondByIdAsync(id);

        if (avond == null)
        {
            return NotFound();
        }

        if (avond.Inschrijvingen.Any())
        {
            TempData["ErrorMessage"] = "Deze avond kan niet worden bewerkt omdat er al inschrijvingen zijn.";
            return RedirectToAction("Index", "BordspellenAvond");
        }

        if (!await _bordspellenAvondService.UserCanEditOrDeleteAsync(id, user.Id))
        {
            return Forbid();
        }

        var bordspellen = await _bordspellenAvondService.GetAllBordspellenAsync();
        ViewBag.Bordspellen = new MultiSelectList(bordspellen, "Id", "Naam", avond.Bordspellen.Select(b => b.Id));

        return View(avond);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(BordspellenAvond model, List<int> geselecteerdeBordspellen)
    {
        var user = await _userManager.GetUserAsync(User);

        if (!await _bordspellenAvondService.UserCanEditOrDeleteAsync(model.Id, user.Id))
        {
            return Forbid();
        }
        model.OrganisatorId = user.Id;

        if (ModelState.IsValid)
        {
            await _bordspellenAvondService.UpdateBordspellenAvondAsync(model, geselecteerdeBordspellen);
            return RedirectToAction(nameof(Index));
        }

        var bordspellen = await _bordspellenAvondService.GetAllBordspellenAsync();
        ViewBag.Bordspellen = new MultiSelectList(bordspellen, "Id", "Naam", geselecteerdeBordspellen);

        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var avond = await _bordspellenAvondService.GetAvondByIdAsync(id);

        if (avond.Inschrijvingen.Any())
        {
            TempData["ErrorMessage"] = "Deze avond kan niet worden verwijderd omdat er al inschrijvingen zijn.";
            return RedirectToAction("Index", "BordspellenAvond");
        }

        if (!await _bordspellenAvondService.UserCanEditOrDeleteAsync(id, user.Id))
        {
            return Forbid();
        }

        return View(avond);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (!await _bordspellenAvondService.UserCanEditOrDeleteAsync(id, user.Id))
        {
            return Forbid();
        }

        await _bordspellenAvondService.DeleteBordspellenAvondAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> BeheerdersOverzicht()
    {
        var userId = _userManager.GetUserId(User);
        var avonden = await _bordspellenAvondService.GetAvondenByOrganisatorAsync(userId);

        for (int i = 0; i < avonden.Count; i++)
        {
            avonden[i] = await _inschrijvingService.GetAvondWithInschrijvingenAndUserNamesAsync(avonden[i].Id);
        }

        return View(avonden);
    }

    public async Task<IActionResult> MijnIngeschrevenAvonden()
    {
        var userId = _userManager.GetUserId(User);
        var ingeschrevenAvonden = await _bordspellenAvondService.GetAvondenWaarIngeschrevenAsync(userId);

        foreach (var avond in ingeschrevenAvonden)
        {
            if (!string.IsNullOrEmpty(avond.OrganisatorId))
            {
                var organisator = await _userManager.FindByIdAsync(avond.OrganisatorId);
                avond.Organisator = organisator;
            }
        }

        return View(ingeschrevenAvonden);
    }
}
