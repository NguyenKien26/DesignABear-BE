using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface ICharacterService
    {
        Task<IEnumerable<CharacterResponse>> GetAllAsync(bool includeInactive = false);
        Task<IEnumerable<CharacterWithAccessoriesResponse>> GetAllWithAccessoriesAsync(bool includeInactive = false);
        Task<CharacterResponse?> GetByIdAsync(Guid characterId, bool includeInactive = false);
        Task<CharacterResponse?> GetBySlugAsync(string slug, bool includeInactive = false);
        Task<CharacterResponse> CreateAsync(CreateCharacterRequest request);
        Task UpdateAsync(Guid characterId, UpdateCharacterRequest request);
        Task DeleteAsync(Guid characterId);
        Task RestoreAsync(Guid characterId);
    }
}
