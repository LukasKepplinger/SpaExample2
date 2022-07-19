
using Bestellservice4.Services.Models;
using Bestellservice4.Shared.Transfer;
using Microsoft.EntityFrameworkCore;

namespace Bestellservice4.Services.IServices
{
    public interface IDishService
    {
        Task<DishDto?> DeleteAsync(int id);
        Task<List<DishDto>> GetAllAsync(string? title = null);
        Task<DishDto?> GetAsync(int id);
        Task<DishDto> InsertAsync(DishDto dishDto);
        Task<DishDto?> UpdateAsync(DishDto dishDto);
    }
}