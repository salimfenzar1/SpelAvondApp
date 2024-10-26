using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpelAvondApp.Application;
using SpelAvondApp.Domain.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SpelAvondApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IBordspellenAvondService _bordspellenAvondService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(IBordspellenAvondService bordspellenAvondService, UserManager<ApplicationUser> userManager)
        {
            _bordspellenAvondService = bordspellenAvondService;
            _userManager = userManager;
        }

        [HttpPost("create/{bordspellenAvondId}")]
        public async Task<IActionResult> CreateReview(int bordspellenAvondId, [FromBody] ReviewCreateDto reviewDto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("Gebruiker niet gevonden.");
            }

            // Controleer of de avond bestaat
            var avond = await _bordspellenAvondService.GetAvondByIdAsync(bordspellenAvondId);
            if (avond == null)
            {
                return NotFound("Bordspellenavond niet gevonden.");
            }

            var review = new Review
            {
                BordspellenAvondId = bordspellenAvondId,
                SpelerId = user.Id,
                Score = reviewDto.Score,
                Opmerking = reviewDto.Opmerking
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bordspellenAvondService.AddReviewAsync(review);
            return Ok("Review succesvol toegevoegd.");
        }



        [HttpGet("avond/{bordspellenAvondId}")]
        public async Task<IActionResult> GetReviewsForAvond(int bordspellenAvondId)
        {
            var avond = await _bordspellenAvondService.GetAvondByIdAsync(bordspellenAvondId);
            if (avond == null)
            {
                return NotFound("Bordspellenavond niet gevonden.");
            }

            var reviews = avond.Reviews.Select(review => new ReviewDto
            {
                Id = review.Id,
                BordspellenAvondId = review.BordspellenAvondId,
                SpelerId = review.SpelerId,
                Score = review.Score,
                Opmerking = review.Opmerking
            }).ToList();

            return Ok(reviews);
        }


        [HttpGet("organisator/{organisatorId}")]
        public async Task<IActionResult> GetReviewsByOrganisator(string organisatorId)
        {
            var organisator = await _userManager.FindByIdAsync(organisatorId);
            if (organisator == null)
            {
                return NotFound("Organisator niet gevonden.");
            }

            var reviews = await _bordspellenAvondService.GetReviewsByOrganisatorAsync(organisatorId);
            var reviewDtos = reviews.Select(review => new ReviewDto
            {
                Id = review.Id,
                BordspellenAvondId = review.BordspellenAvondId,
                SpelerId = review.SpelerId,
                Score = review.Score,
                Opmerking = review.Opmerking
            }).ToList();

            return Ok(reviewDtos);
        }
    }
}
