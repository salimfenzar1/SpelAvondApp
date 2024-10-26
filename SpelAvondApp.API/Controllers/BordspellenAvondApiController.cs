using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpelAvondApp.Application;
using SpelAvondApp.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpelAvondApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BordspellenAvondApiController : ControllerBase
    {
        private readonly IBordspellenAvondService _bordspellenAvondService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BordspellenAvondApiController(IBordspellenAvondService bordspellenAvondService, UserManager<ApplicationUser> userManager)
        {
            _bordspellenAvondService = bordspellenAvondService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<BordspellenAvondDto>>> GetAllAvonden()
        {
            var avonden = await _bordspellenAvondService.GetAllAvondenAsync();
            var avondenDto = avonden.Select(avond => new BordspellenAvondDto
            {
                Id = avond.Id,
                Adres = avond.Adres,
                Datum = avond.Datum,
                MaxAantalSpelers = avond.MaxAantalSpelers,
                Is18Plus = avond.Is18Plus,
                OrganisatorUsername = avond.Organisator?.UserName,
                InschrijvingenUsernames = avond.Inschrijvingen.Select(i => i.Speler?.UserName).ToList(),
                BordspelNamen = avond.Bordspellen.Select(b => b.Naam).ToList(),
                GemiddeldeScore = avond.GemiddeldeScore
            }).ToList();

            return Ok(avondenDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BordspellenAvondDto>> GetAvondById(int id)
        {
            var avond = await _bordspellenAvondService.GetAvondByIdAsync(id);
            if (avond == null)
            {
                return NotFound("Bordspellenavond niet gevonden.");
            }

            var avondDto = new BordspellenAvondDto
            {
                Id = avond.Id,
                Adres = avond.Adres,
                Datum = avond.Datum,
                MaxAantalSpelers = avond.MaxAantalSpelers,
                Is18Plus = avond.Is18Plus,
                OrganisatorUsername = avond.Organisator?.UserName,
                InschrijvingenUsernames = avond.Inschrijvingen.Select(i => i.Speler?.UserName).ToList(),
                BordspelNamen = avond.Bordspellen.Select(b => b.Naam).ToList(),
                GemiddeldeScore = avond.GemiddeldeScore

            };

            return Ok(avondDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBordspellenAvondDto model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                if (!await _bordspellenAvondService.IsUserEligibleToOrganizeAsync(user))
                {
                    return BadRequest("Je moet minimaal 18 jaar oud zijn om een bordspellenavond te organiseren.");
                }
            }

            

            var bordspellenAvond = new BordspellenAvond
            {
                Adres = model.Adres,
                Datum = model.Datum,
                MaxAantalSpelers = model.MaxAantalSpelers,
                Is18Plus = model.Is18Plus,
                BiedtLactosevrijeOpties = model.BiedtLactosevrijeOpties,
                BiedtNotenvrijeOpties = model.BiedtNotenvrijeOpties,
                BiedtVegetarischeOpties = model.BiedtVegetarischeOpties,
                BiedtAlcoholvrijeOpties = model.BiedtAlcoholvrijeOpties,
                OrganisatorId = user.Id
            };

            await _bordspellenAvondService.CreateBordspellenAvondAsync(bordspellenAvond, model.BordspelIds);
            return CreatedAtAction(nameof(GetAvondById), new { id = bordspellenAvond.Id }, new
            {
                Message = "Bordspellenavond succesvol aangemaakt.",
                BordspellenAvond = bordspellenAvond
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateBordspellenAvondDto model)
        {
            var existingAvond = await _bordspellenAvondService.GetAvondByIdAsync(id);
            if (existingAvond == null)
            {
                return NotFound("Bordspellenavond niet gevonden.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (existingAvond.OrganisatorId != user.Id)
            {
                return Forbid("Alleen de organisator kan de avond bewerken.");
            }

            if (existingAvond.Inschrijvingen.Any())
            {
                return BadRequest("De avond kan niet worden bijgewerkt omdat er al inschrijvingen zijn.");
            }

            existingAvond.Adres = model.Adres;
            existingAvond.Datum = model.Datum;
            existingAvond.MaxAantalSpelers = model.MaxAantalSpelers;
            existingAvond.Is18Plus = model.Is18Plus;
            existingAvond.BiedtLactosevrijeOpties = model.BiedtLactosevrijeOpties;
            existingAvond.BiedtNotenvrijeOpties = model.BiedtNotenvrijeOpties;
            existingAvond.BiedtVegetarischeOpties = model.BiedtVegetarischeOpties;
            existingAvond.BiedtAlcoholvrijeOpties = model.BiedtAlcoholvrijeOpties;

            await _bordspellenAvondService.UpdateBordspellenAvondAsync(existingAvond, model.BordspelIds);

            return Ok(new { Message = "Bordspellenavond succesvol bijgewerkt." });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var avond = await _bordspellenAvondService.GetAvondByIdAsync(id);
            if (avond == null)
            {
                return NotFound("Bordspellenavond niet gevonden.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (avond.OrganisatorId != user.Id)
            {
                return Forbid("Alleen de organisator kan de avond verwijderen.");
            }

            if (avond.Inschrijvingen.Any())
            {
                return BadRequest("De avond kan niet worden verwijderd omdat er al inschrijvingen zijn.");
            }

            await _bordspellenAvondService.DeleteBordspellenAvondAsync(id);

            return Ok(new { Message = "Bordspellenavond succesvol verwijderd." });
        }
    }
}
