// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpelAvondApp.Domain.Models;

namespace SpelAvondApp.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            public string Naam { get; set; }
            public string Geslacht { get; set; }
            public string Straat { get; set; }
            public string Huisnummer { get; set; }
            public string Stad { get; set; }
            public DateTime Geboortedatum { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Input = new InputModel
            {
                Naam = user.Naam,
                Geslacht = user.Geslacht,
                Straat = user.Straat,
                Huisnummer = user.Huisnummer,
                Stad = user.Stad,
                Geboortedatum = user.Geboortedatum
            };

            Username = userName;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Controleer of de geboortedatum niet in de toekomst ligt
            if (Input.Geboortedatum > DateTime.Today)
            {
                ModelState.AddModelError(string.Empty, "De geboortedatum mag niet in de toekomst liggen.");
                await LoadAsync(user);
                return Page();
            }

            // Bereken de leeftijd van de gebruiker
            var today = DateTime.Today;
            var age = today.Year - Input.Geboortedatum.Year;
            if (Input.Geboortedatum.Date > today.AddYears(-age)) age--;

            // Controleer of de gebruiker minstens 16 jaar oud is
            if (age < 16)
            {
                ModelState.AddModelError(string.Empty, "Je moet minimaal 16 jaar oud zijn om een account aan te maken.");
                await LoadAsync(user);
                return Page();
            }

          

            // Update de extra profielvelden (Naam, Geslacht, Straat, etc.)
            if (Input.Naam != user.Naam) user.Naam = Input.Naam;
            if (Input.Geslacht != user.Geslacht) user.Geslacht = Input.Geslacht;
            if (Input.Straat != user.Straat) user.Straat = Input.Straat;
            if (Input.Huisnummer != user.Huisnummer) user.Huisnummer = Input.Huisnummer;
            if (Input.Stad != user.Stad) user.Stad = Input.Stad;
            if (Input.Geboortedatum != user.Geboortedatum) user.Geboortedatum = Input.Geboortedatum;

            // Profiel bijwerken
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to update profile.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

    }
}
