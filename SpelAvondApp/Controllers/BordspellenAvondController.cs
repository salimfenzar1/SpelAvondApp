using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpelAvondApp.Domain.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SpelAvondApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class BordspellenAvondController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SpellenDbContext _context;

    public BordspellenAvondController(UserManager<ApplicationUser> userManager, SpellenDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

        public async Task<IActionResult> Index()
    {
        var avonden = await _context.BordspellenAvonden
            .Include(b => b.Organisator)
            .Include(b => b.Bordspellen)
            .ToListAsync();
        return View(avonden);
    }

    public IActionResult Create()
    {
        var bordspellen = _context.Bordspellen.ToList();
        ViewBag.Bordspellen = bordspellen; // Stuur de bordspellen mee naar de view
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(BordspellenAvond model, List<int> geselecteerdeBordspellen)
    {
        var user = await _userManager.GetUserAsync(User);

        // Leeftijdscontrole
        var today = DateTime.Today;
        var age = today.Year - user.Geboortedatum.Year;
        if (user.Geboortedatum.Date > today.AddYears(-age)) age--;

        if (age < 18)
        {
            return BadRequest("Je moet minimaal 18 jaar oud zijn om een bordspellenavond te organiseren.");
        }

        if (ModelState.IsValid)
        {
            model.OrganisatorId = user.Id;

            // Voeg de geselecteerde bordspellen toe aan de avond
            model.Bordspellen = _context.Bordspellen
                .Where(b => geselecteerdeBordspellen.Contains(b.Id))
                .ToList();

            _context.BordspellenAvonden.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        var bordspellen = _context.Bordspellen.ToList();
        ViewBag.Bordspellen = bordspellen;
        return View(model);
    }


}
