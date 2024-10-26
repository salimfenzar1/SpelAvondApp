using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpelAvondApp.Application;
using SpelAvondApp.Domain.Models;
using System.Collections.Generic;
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
        public async Task<ActionResult<List<BordspellenAvond>>> GetAllAvonden()
        {
            var avonden = await _bordspellenAvondService.GetAllAvondenAsync();
            Console.WriteLine($"Aantal bordspelavonden in controller: {avonden.Count}");
            return Ok(avonden);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BordspellenAvond model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (!await _bordspellenAvondService.IsUserEligibleToOrganizeAsync(user))
            {
                return BadRequest("Je moet minimaal 18 jaar oud zijn om een bordspellenavond te organiseren.");
            }

            model.OrganisatorId = user.Id;

            await _bordspellenAvondService.CreateBordspellenAvondAsync(model, new List<int>());
            return CreatedAtAction(nameof(GetAvondById), new { id = model.Id }, model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BordspellenAvond>> GetAvondById(int id)
        {
            var avond = await _bordspellenAvondService.GetAvondByIdAsync(id);
            if (avond == null)
            {
                return NotFound();
            }
            return Ok(avond);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BordspellenAvond model)
        {
            if (id != model.Id)
            {
                return BadRequest("ID mismatch.");
            }

            await _bordspellenAvondService.UpdateBordspellenAvondAsync(model, new List<int>());
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bordspellenAvondService.DeleteBordspellenAvondAsync(id);
            return NoContent();
        }
    }
}
