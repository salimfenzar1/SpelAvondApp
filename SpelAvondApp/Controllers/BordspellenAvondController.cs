﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpelAvondApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorize]
public class BordspellenAvondController : Controller
{
    private readonly IBordspellenAvondService _bordspellenAvondService;
    private readonly UserManager<ApplicationUser> _userManager;

    public BordspellenAvondController(IBordspellenAvondService bordspellenAvondService, UserManager<ApplicationUser> userManager)
    {
        _bordspellenAvondService = bordspellenAvondService;
        _userManager = userManager;
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
            return BadRequest("Er is geen geregistreerde gebruiker gevonden.");
        }

        if (!await _bordspellenAvondService.IsUserEligibleToOrganizeAsync(user))
        {
            return BadRequest("Je moet minimaal 18 jaar oud zijn om een bordspellenavond te organiseren.");
        }

        model.OrganisatorId = user.Id;
        ModelState.Remove("Organisator");

        if (ModelState.IsValid)
        {
            await _bordspellenAvondService.CreateBordspellenAvondAsync(model, geselecteerdeBordspellen);
            return RedirectToAction(nameof(Index));
        }
        else
        {
            // Print de ModelState-fouten naar de console
            foreach (var modelStateKey in ModelState.Keys)
            {
                var modelStateVal = ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    Console.WriteLine($"ModelState error: Key='{modelStateKey}', Error='{error.ErrorMessage}'");
                }
            }
        }

        var bordspellen = await _bordspellenAvondService.GetAllBordspellenAsync();
        ViewBag.Bordspellen = new MultiSelectList(bordspellen, "Id", "Naam");
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (!await _bordspellenAvondService.UserCanEditOrDeleteAsync(id, user.Id))
        {
            return Forbid();
        }

        var avond = await _bordspellenAvondService.GetAvondByIdAsync(id);

        if (avond == null)
        {
            return NotFound();
        }

        var bordspellen = await _bordspellenAvondService.GetAllBordspellenAsync();
        ViewBag.Bordspellen = new MultiSelectList(bordspellen, "Id", "Naam", avond.Bordspellen.Select(b => b.Id));

        // Voeg het organisatorId toe aan het model, indien nodig.
        avond.OrganisatorId = user.Id;

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
        if (!await _bordspellenAvondService.UserCanEditOrDeleteAsync(id, user.Id))
        {
            return Forbid();
        }

        var avond = await _bordspellenAvondService.GetAvondByIdAsync(id);
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
}
