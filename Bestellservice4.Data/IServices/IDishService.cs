
using Bestellservice4.Services.Models;
using Bestellservice4.Shared.Helper;
using Bestellservice4.Shared.Params;
using Bestellservice4.Shared.Transfer;
using Microsoft.EntityFrameworkCore;

namespace Bestellservice4.Services.IServices
{
    public interface IDishService
    {
        Task<List<DishDto>?> GetAllAsync(string? title = null);
        Task<PageOf<DishDto>?> GetPageAsync(PageParams parameters);
        Task<DishDto?> GetAsync(int id);
        Task<DishCeDto> InsertAsync(DishCeDto dishDto);
        Task<DishCeDto?> UpdateAsync(DishCeDto dishDto);
        Task<DishCeDto?> DeleteAsync(int id);
    }
}