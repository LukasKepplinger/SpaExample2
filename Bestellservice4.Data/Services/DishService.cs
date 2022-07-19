
using Bestellservice4.Services.IServices;
using Bestellservice4.Services.Models;
using Bestellservice4.Shared.Params;
using Bestellservice4.Shared.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellservice4.Services.Services
{
    public class DishService : IDishService
    {
        private readonly ApplicationDbContext context;
        private DbSet<Dish> Dishes { get; init; }

        public DishService(ApplicationDbContext context)
        {
            Dishes = context.Dishes;
            this.context = context;
        }

        public async Task<List<DishDto>> GetAllAsync(string? title = null) //simple search parameter
        {
            var dishes = Dishes.AsNoTracking()
                .Include(dish => dish.Allergens)
                .Select(dish => new DishDto
                {
                    Id = dish.Id,
                    Title = dish.Title,
                    Description = dish.Description,
                    Created = dish.Created,
                    Price = dish.Price,
                    ImageData = dish.ImageData,
                    Allergens = dish.Allergens.Select(dishAllergen => new AllergenDto
                    {
                        Id = dishAllergen.Id,
                        Letter = dishAllergen.Letter,
                        Title = dishAllergen.Title,
                        Description = dishAllergen.Description
                    }).ToList()
                }).AsQueryable();

            if (!string.IsNullOrEmpty(title))
                dishes = dishes.Where(dish => dish.Title.Contains(title));

            return await dishes.ToListAsync();
        }

        public async Task<DishDto?> GetAsync(int id)
        {
            return await Dishes.AsNoTracking()
                .Include(dish => dish.Allergens)
                .Select(dish => new DishDto
                {
                    Id = dish.Id,
                    Title = dish.Title,
                    Description = dish.Description,
                    Created = dish.Created,
                    Price = dish.Price,
                    ImageData = dish.ImageData,
                    Allergens = dish.Allergens.Select(dishAllergen => new AllergenDto
                    {
                        Id = dishAllergen.Id,
                        Letter = dishAllergen.Letter,
                        Title = dishAllergen.Title,
                        Description = dishAllergen.Description
                    }).ToList()
                }).FirstOrDefaultAsync(dish => dish.Id == id);
        }

        public async Task<List<DishDto>> GetAsync(DishParams parameters) //Paging
        {
            var dishes = Dishes.AsNoTracking()
               .OrderBy(dish => dish.Created)
               .Skip((parameters.CurrentPage - 1) * parameters.PageSize)
               .Take(parameters.PageSize)
               .Include(dish => dish.Allergens)
               .Select(dish => new DishDto
               {
                   Id = dish.Id,
                   Title = dish.Title,
                   Description = dish.Description,
                   Created = dish.Created,
                   Price = dish.Price,
                   ImageData = dish.ImageData,
                   Allergens = dish.Allergens.Select(dishAllergen => new AllergenDto
                   {
                       Id = dishAllergen.Id,
                       Letter = dishAllergen.Letter,
                       Title = dishAllergen.Title,
                       Description = dishAllergen.Description
                   }).ToList()
               }).AsQueryable();

            return await dishes.ToListAsync();
        }

        public async Task<DishDto> InsertAsync(DishDto dishDto)
        {
            var dish = new Dish
            {
                Id = dishDto.Id,
                Title = dishDto.Title,
                Description = dishDto.Description,
                Created = DateTime.Now,
                Price = dishDto.Price,
                ImageData = dishDto.ImageData,
            };

            await Dishes.AddAsync(dish);
            await context.SaveChangesAsync();
            dishDto.Id = dish.Id;
            return dishDto;
        }

        public async Task<DishDto?> UpdateAsync(DishDto dishDto)
        {
            //if (await Dishes.FindAsync(dishDto.Id) == null)
            //    return null;

            var dish = new Dish
            {
                Id = dishDto.Id,
                Title = dishDto.Title,
                Description = dishDto.Description,
                Price = dishDto.Price,
                ImageData = dishDto.ImageData
            };

            var table = Dishes.Attach(dish);
            table.State = EntityState.Modified;
            await context.SaveChangesAsync();
            dishDto.Id = dish.Id;
            return dishDto;
        }

        public async Task<DishDto?> DeleteAsync(int id)
        {
            Dish? dish = await Dishes.FindAsync(id);

            if (dish == null)
                return null;

            Dishes.Remove(dish);
            await context.SaveChangesAsync();

            return new DishDto
            {
                Id = dish.Id,
                Title = dish.Title,
                Description = dish.Description,
                Created = dish.Created,
                Price = dish.Price,
                ImageData = dish.ImageData
            };
        }
    }
}
