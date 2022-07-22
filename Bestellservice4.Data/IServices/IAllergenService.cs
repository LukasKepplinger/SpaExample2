

using Bestellservice4.Shared.Transfer;

namespace Bestellservice4.Services.IServices
{
    public interface IAllergenService
    {
        Task<List<AllergenDto>> GetAllAsync();
        Task<List<AllergenDto>> GetForDishAsync(int dishId);
        Task<AllergenDto?> GetAsync(int id);
        Task<AllergenDto> InsertAsync(AllergenDto allergenDto);
        Task<AllergenDto> UpdateAsync(AllergenDto allergenDto);
        Task<AllergenDto?> DeleteAsync(int id);
    }
}