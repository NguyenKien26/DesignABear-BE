using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;

namespace DesignABear.Services.Services.Implementations
{
    public class CharacterService : ICharacterService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CharacterService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CharacterResponse>> GetAllAsync(bool includeInactive = false)
        {
            var characters = await _uow.Characters.GetAllAsync();
            if (!includeInactive)
            {
                characters = characters.Where(c => c.IsActive).ToList();
            }
            return _mapper.Map<IEnumerable<CharacterResponse>>(characters);
        }

        public async Task<IEnumerable<CharacterWithAccessoriesResponse>> GetAllWithAccessoriesAsync(bool includeInactive = false)
        {
            var characters = await _uow.Characters.GetAllWithAccessoriesAsync();
            if (!includeInactive)
            {
                characters = characters.Where(c => c.IsActive).ToList();
            }
            
            return characters.Select(c => new CharacterWithAccessoriesResponse
            {
                CharacterId = c.CharacterId,
                Name = c.Name,
                Slug = c.Slug,
                LicenseBrand = c.LicenseBrand,
                ImageUrl = c.ImageUrl,
                Description = c.Description,
                Products = c.ProductCharacters.Select(pc => new ProductWithAccessoriesResponse
                {
                    ProductId = pc.Product.ProductId,
                    Name = pc.Product.Name,
                    Accessories = _mapper.Map<List<PersonalizationRuleResponse>>(pc.Product.BasePersonalizationRules)
                }).ToList()
            });
        }

        public async Task<CharacterResponse?> GetByIdAsync(Guid characterId, bool includeInactive = false)
        {
            var character = await _uow.Characters.GetByIdAsync(characterId);
            if (character == null || (!includeInactive && !character.IsActive))
            {
                throw new NotFoundException("Character", characterId);
            }
            return _mapper.Map<CharacterResponse>(character);
        }

        public async Task<CharacterResponse?> GetBySlugAsync(string slug, bool includeInactive = false)
        {
            var character = await _uow.Characters.GetBySlugAsync(slug);
            if (character == null || (!includeInactive && !character.IsActive))
            {
                throw new NotFoundException("Character", slug);
            }
            return _mapper.Map<CharacterResponse>(character);
        }

        public async Task<CharacterResponse> CreateAsync(CreateCharacterRequest request)
        {
            var character = _mapper.Map<Character>(request);
            character.CharacterId = Guid.NewGuid();
            character.IsActive = true;
            
            await _uow.Characters.AddAsync(character);
            await _uow.SaveChangesAsync();
            
            return _mapper.Map<CharacterResponse>(character);
        }

        public async Task UpdateAsync(Guid characterId, UpdateCharacterRequest request)
        {
            var character = await _uow.Characters.GetByIdAsync(characterId);
            if (character == null) throw new NotFoundException("Character", characterId);
            
            _mapper.Map(request, character);
            
            _uow.Characters.Update(character);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid characterId)
        {
            var character = await _uow.Characters.GetByIdAsync(characterId);
            if (character == null) throw new NotFoundException("Character", characterId);
            
            character.IsActive = false;
            _uow.Characters.Update(character);
            await _uow.SaveChangesAsync();
        }

        public async Task RestoreAsync(Guid characterId)
        {
            var character = await _uow.Characters.GetByIdAsync(characterId);
            if (character == null) throw new NotFoundException("Character", characterId);
            
            character.IsActive = true;
            _uow.Characters.Update(character);
            await _uow.SaveChangesAsync();
        }
    }
}
