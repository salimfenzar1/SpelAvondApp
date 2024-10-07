using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpelAvondApp.Domain.Models;
using System.Threading.Tasks;

[Authorize]
public class BordspelController : Controller
{
    private readonly IBordspelService _bordspelService;
    private readonly UserManager<ApplicationUser> _userManager;

    public BordspelController(IBordspelService bordspelService, UserManager<ApplicationUser> userManager)
    {
        _bordspelService = bordspelService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var bordspellen = await _bordspelService.GetAllBordspellenAsync();
        return View(bordspellen);
    }

    public IActionResult CreateBordspel()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateBordspel(Bordspel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (!await _bordspelService.IsUserEligibleToCreateBordspel(user))
        {
            return BadRequest("Je moet minimaal 18 jaar oud zijn om een bordspel aan te maken.");
        }

        if (ModelState.IsValid)
        {
            await _bordspelService.AddBordspelAsync(model);
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    public async Task<IActionResult> EditBordspel(int id)
    {
        var bordspel = await _bordspelService.GetBordspelByIdAsync(id);
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
            await _bordspelService.UpdateBordspelAsync(model);
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    public async Task<IActionResult> DeleteBordspel(int id)
    {
        var bordspel = await _bordspelService.GetBordspelByIdAsync(id);
        if (bordspel == null)
        {
            return NotFound();
        }
        return View(bordspel);
    }

    [HttpPost, ActionName("DeleteBordspel")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _bordspelService.DeleteBordspelAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
