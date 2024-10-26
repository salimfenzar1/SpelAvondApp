
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpelAvondApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpelAvondApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BordspelApiController : ControllerBase
    {
        private readonly IBordspelService _bordspelService;

        public BordspelApiController(IBordspelService bordspelService)
        {
            _bordspelService = bordspelService;
        }

        // GET: api/Bordspel
        [HttpGet]
        [HttpGet]
        public async Task<ActionResult<List<BordspelDto>>> GetAllBordspellen()
        {
            var bordspellen = await _bordspelService.GetAllBordspellenAsync();
            var bordspellenDto = bordspellen.Select(b => new BordspelDto
            {
                Id = b.Id,
                Naam = b.Naam,
                Beschrijving = b.Beschrijving,
                Genre = b.Genre.ToString(),
                Is18Plus = b.Is18Plus,
                SoortSpel = b.SoortSpel.ToString(),
                FotoPath = b.FotoPath
            }).ToList();

            return Ok(bordspellenDto);
        }


        // GET: api/Bordspel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BordspelDto>> GetBordspelById(int id)
        {
            var bordspel = await _bordspelService.GetBordspelByIdAsync(id);
            if (bordspel == null)
            {
                return NotFound("Bordspel niet gevonden.");
            }

            var bordspelDto = new BordspelDto
            {
                Id = bordspel.Id,
                Naam = bordspel.Naam,
                Beschrijving = bordspel.Beschrijving,
                Genre = bordspel.Genre.ToString(),
                Is18Plus = bordspel.Is18Plus,
                SoortSpel = bordspel.SoortSpel.ToString(),
                FotoPath = bordspel.FotoPath
            };

            return Ok(bordspelDto);
        }


        [HttpPost]
        public async Task<ActionResult> CreateBordspel([FromBody] CreateBordspelDto model)
        {
            var bordspel = new Bordspel
            {
                Naam = model.Naam,
                Beschrijving = model.Beschrijving,
                Genre = model.Genre,
                Is18Plus = model.Is18Plus,
                SoortSpel = model.SoortSpel,
                FotoPath = model.FotoPath
            };

            await _bordspelService.AddBordspelAsync(bordspel);
            return CreatedAtAction(nameof(GetBordspelById), new { id = bordspel.Id }, new
            {
                Message = "Bordspel succesvol aangemaakt.",
                Bordspel = bordspel
            });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBordspel(int id, [FromBody] CreateBordspelDto model)
        {
            var existingBordspel = await _bordspelService.GetBordspelByIdAsync(id);
            if (existingBordspel == null)
            {
                return NotFound("Bordspel niet gevonden.");
            }

            existingBordspel.Naam = model.Naam;
            existingBordspel.Beschrijving = model.Beschrijving;
            existingBordspel.Genre = model.Genre;
            existingBordspel.Is18Plus = model.Is18Plus;
            existingBordspel.SoortSpel = model.SoortSpel;
            existingBordspel.FotoPath = model.FotoPath;

            await _bordspelService.UpdateBordspelAsync(existingBordspel);
            return Ok(new { Message = "Bordspel succesvol bijgewerkt." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBordspel(int id)
        {
            var bordspel = await _bordspelService.GetBordspelByIdAsync(id);
            if (bordspel == null)
            {
                return NotFound();
            }

            await _bordspelService.DeleteBordspelAsync(id);
            return NoContent();
        }
    }
}
