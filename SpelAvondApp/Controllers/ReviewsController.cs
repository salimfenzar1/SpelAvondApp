using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpelAvondApp.Application;
using SpelAvondApp.Domain.Models;
using System.Threading.Tasks;

[Authorize]
public class ReviewsController : Controller
{
    private readonly IBordspellenAvondService _bordspellenAvondService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReviewsController(IBordspellenAvondService bordspellenAvondService, UserManager<ApplicationUser> userManager)
    {
        _bordspellenAvondService = bordspellenAvondService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int bordspellenAvondId)
    {
        var avond = await _bordspellenAvondService.GetAvondByIdAsync(bordspellenAvondId);
        if (avond == null)
        {
            return NotFound();
        }

        var review = new Review
        {
            BordspellenAvondId = bordspellenAvondId
        };

        return View(review);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Review review)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest("Gebruiker niet gevonden.");
        }
        review.SpelerId = user.Id;
        if (ModelState.IsValid)
        {
            await _bordspellenAvondService.AddReviewAsync(review);  
            return RedirectToAction("MijnIngeschrevenAvonden", "BordspellenAvond");
        }

        return View(review);
    }

    [HttpGet]
public async Task<IActionResult> OrganisatorReviews(string organisatorId)
    {
        var organisator = await _userManager.FindByIdAsync(organisatorId);
        if (organisator == null)
        {
            return NotFound("Organisator niet gevonden.");
        }

        var reviews = await _bordspellenAvondService.GetReviewsByOrganisatorAsync(organisatorId);

        ViewBag.OrganisatorNaam = organisator.UserName;
        return View(reviews);
    }
}
