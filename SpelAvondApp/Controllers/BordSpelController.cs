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
    private readonly IWebHostEnvironment _webHostEnvironment;


    public BordspelController(IBordspelService bordspelService, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
    {
        _bordspelService = bordspelService;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
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
    public async Task<IActionResult> CreateBordspel(Bordspel model, IFormFile foto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (!await _bordspelService.IsUserEligibleToCreateBordspel(user))
        {
            return BadRequest("Je moet minimaal 18 jaar oud zijn om een bordspel aan te maken.");
        }

        if (ModelState.IsValid)
        {
            var fotoPath = await SavePhotoAsync(foto);
            if (fotoPath != null)
            {
                model.FotoPath = fotoPath;
            }

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
    public async Task<IActionResult> EditBordspel(Bordspel model, IFormFile foto)
    {
        if (ModelState.IsValid)
        {
            if (foto != null && foto.Length > 0)
            {
                var fotoPath = await SavePhotoAsync(foto);
                if (fotoPath != null)
                {
                    model.FotoPath = fotoPath;
                }
            }

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
    private async Task<string> SavePhotoAsync(IFormFile foto)
    {
        if (foto != null && foto.Length > 0)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + foto.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            Directory.CreateDirectory(uploadsFolder); // Zorg ervoor dat de map bestaat

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await foto.CopyToAsync(fileStream);
            }

            return "/uploads/" + uniqueFileName;
        }
        return null;
    }
}

