using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpelAvondApp.Data;
using SpelAvondApp.Infrastructure;
using SpelAvondApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class BordspelController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SpellenDbContext _context;

    public BordspelController(UserManager<ApplicationUser> userManager, SpellenDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public IActionResult CreateBordspel()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateBordspel(Bordspel model)
    {
        var user = await _userManager.GetUserAsync(User);
        var today = DateTime.Today;
        var age = today.Year - user.Geboortedatum.Year;
        if (user.Geboortedatum.Date > today.AddYears(-age)) age--;

        if (age < 18)
        {
            return BadRequest("Je moet minimaal 18 jaar oud zijn om een bordspel aan te maken.");
        }

        if (ModelState.IsValid)
        {
            _context.Bordspellen.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

 
    public IActionResult Index()
    {
        var bordspellen = _context.Bordspellen.ToList();
        return View(bordspellen);
    }

    public IActionResult EditBordspel(int id)
    {
        var bordspel = _context.Bordspellen.FirstOrDefault(b => b.Id == id);
        if (bordspel == null)
        {
            return NotFound();
        }
        return View(bordspel);
    }

    [HttpPost]
    public async Task<IActionResult> EditBordspel(Bordspel model)
    {
        if (ModelState.IsValid)
        {
            var existingBordspel = _context.Bordspellen.FirstOrDefault(b => b.Id == model.Id);
            if (existingBordspel == null)
            {
                return NotFound();
            }

            existingBordspel.Naam = model.Naam;
            existingBordspel.Beschrijving = model.Beschrijving;
            existingBordspel.Genre = model.Genre;
            existingBordspel.Is18Plus = model.Is18Plus;
            existingBordspel.SoortSpel = model.SoortSpel;

            _context.Bordspellen.Update(existingBordspel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    public IActionResult DeleteBordspel(int id)
    {
        var bordspel = _context.Bordspellen.FirstOrDefault(b => b.Id == id);
        if (bordspel == null)
        {
            return NotFound();
        }
        return View(bordspel);
    }

    [HttpPost, ActionName("DeleteBordspel")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var bordspel = _context.Bordspellen.FirstOrDefault(b => b.Id == id);
        if (bordspel == null)
        {
            return NotFound();
        }

        _context.Bordspellen.Remove(bordspel);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
