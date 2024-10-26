
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
        public async Task<ActionResult<IEnumerable<Bordspel>>> GetAllBordspellen()
        {
            var bordspellen = await _bordspelService.GetAllBordspellenAsync();
            return Ok(bordspellen);
        }

        // GET: api/Bordspel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bordspel>> GetBordspel(int id)
        {
            var bordspel = await _bordspelService.GetBordspelByIdAsync(id);

            if (bordspel == null)
            {
                return NotFound();
            }

            return Ok(bordspel);
        }

        // POST: api/Bordspel
        [HttpPost]
        public async Task<ActionResult> CreateBordspel(Bordspel bordspel)
        {
            await _bordspelService.AddBordspelAsync(bordspel);
            return CreatedAtAction(nameof(GetBordspel), new { id = bordspel.Id }, bordspel);
        }

        // PUT: api/Bordspel/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBordspel(int id, Bordspel bordspel)
        {
            if (id != bordspel.Id)
            {
                return BadRequest("De ID's komen niet overeen.");
            }

            var existingBordspel = await _bordspelService.GetBordspelByIdAsync(id);
            if (existingBordspel == null)
            {
                return NotFound();
            }

            await _bordspelService.UpdateBordspelAsync(bordspel);
            return NoContent();
        }

        // DELETE: api/Bordspel/5
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
