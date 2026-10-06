using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : BaseController
    {
        private readonly ICharacterService _characterService;

        public CharactersController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var isAdmin = GetCurrentRoleId() == 1;
            var characters = await _characterService.GetAllAsync(includeInactive: isAdmin);
            return HandleResult(Result<IEnumerable<CharacterResponse>>.Success(characters));
        }

        [HttpGet("all-with-accessories")]
        public async Task<IActionResult> GetAllWithAccessories()
        {
            var isAdmin = GetCurrentRoleId() == 1;
            var characters = await _characterService.GetAllWithAccessoriesAsync(includeInactive: isAdmin);
            return HandleResult(Result<IEnumerable<CharacterWithAccessoriesResponse>>.Success(characters));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var isAdmin = GetCurrentRoleId() == 1;
            var character = await _characterService.GetByIdAsync(id, includeInactive: isAdmin);
            if (character == null)
            {
                return HandleResult(Result<CharacterResponse>.Failure(new Error("NotFound", $"Character with ID {id} not found.")));
            }
            return HandleResult(Result<CharacterResponse>.Success(character));
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var isAdmin = GetCurrentRoleId() == 1;
            var character = await _characterService.GetBySlugAsync(slug, includeInactive: isAdmin);
            if (character == null)
            {
                return HandleResult(Result<CharacterResponse>.Failure(new Error("NotFound", $"Character with slug {slug} not found.")));
            }
            return HandleResult(Result<CharacterResponse>.Success(character));
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create([FromBody] CreateCharacterRequest request)
        {
            var character = await _characterService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = character.CharacterId }, Result<CharacterResponse>.Success(character));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCharacterRequest request)
        {
            await _characterService.UpdateAsync(id, request);
            return HandleResult(Result.Success());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _characterService.DeleteAsync(id);
            return HandleResult(Result.Success());
        }

        [HttpPost("{id}/restore")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Restore(Guid id)
        {
            await _characterService.RestoreAsync(id);
            return HandleResult(Result.Success());
        }
    }
}
