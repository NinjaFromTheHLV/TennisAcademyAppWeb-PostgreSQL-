using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Racket;

namespace TennisAcademyApp.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RacketController : ControllerBase
    {
        private readonly IRacketService racketService;
        public RacketController(IRacketService racketService)
        {
            this.racketService = racketService;
        }
        protected string? GetUserId()
        {
            string? userId = null;
                userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return userId;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRackets()
        {
            var rackets = await this.racketService.GetAllRacketsAsync();
            return Ok(rackets);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRacketById(int id)
        {
            try
            {
                var racket = await this.racketService.FindRacketByIdAsync(id);
                return Ok(racket);
            }
            catch (ArgumentException ex)
            {
                return NotFound();
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateRacket([FromBody] RacketCreateInputModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(model);
            }
            var userId = GetUserId();
            if (userId == null)
            {
                return NotFound("User not found.");
            }
            var result = await racketService.AddRacketAsync(userId, model);
            if (!result)
            {
                return BadRequest("Failed to create racket.");
            }
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditRacket([FromBody] RacketEditFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(model);
            }
            var userId = GetUserId();
            if (userId == null)
            {
                return NotFound("User not found.");
            }
            var existingRacket = await racketService.FindRacketByIdAsync(model.Id);
            if (existingRacket == null)
            {
                return NotFound("Racket not found.");
            }
            var result = await racketService.EditRacketAsync(model);
            if (!result)
            {
                 return BadRequest("Failed to edit racket.");
            }
            return Ok(model);
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRacket(int id)
        {
            string userId = GetUserId();
            if (userId == null)
            {
                return NotFound("User not found.");
            }
            var existingRacket = await racketService.FindRacketByIdAsync(id);
            if (existingRacket == null)
            {
                return NotFound("Racket not found.");
            }
            var result = await racketService.DeleteRacketAsync(userId, id);
            if (!result)
            {
                return BadRequest("Failed to delete racket.");
            }
            return Ok("Racket deleted successfully");
        }
    }
}
