using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface ICharacterRepository : IGenericRepository<Character>
    {
        Task<Character?> GetBySlugAsync(string slug);
        Task<IEnumerable<Character>> GetAllWithAccessoriesAsync();
    }
}
