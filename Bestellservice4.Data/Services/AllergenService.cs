
using Bestellservice4.Services.IServices;
using Bestellservice4.Services.Models;
using Bestellservice4.Shared.Transfer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellservice4.Services.Services
{
    public class AllergenService : IAllergenService
    {
        public ApplicationDbContext context { get; }
        private DbSet<Allergen> Allergens { get; init; }
        public AllergenService(ApplicationDbContext context)
        {
            this.context = context;
            Allergens = context.Allergens;
        }

        public async Task<List<AllergenDto>> GetAllAsync()
        {
            return await Allergens
                .AsNoTracking()
                .OrderBy(a => a.Letter)
                .Select(a => new AllergenDto
                {
                    Id = a.Id,
                    Letter = a.Letter,
                    Title = a.Title,
                    Description = a.Description,
                    DishId = a.Dish.Id
                }).ToListAsync();
        }

        public async Task<List<AllergenDto>> GetForDishAsync(int dishId)
        {
            return await Allergens
                .AsNoTracking()
                .Where(a => a.Dish.Id == dishId)
                .OrderBy(a => a.Letter)
                .Select(a => new AllergenDto
                {
                    Id = a.Id,
                    Letter = a.Letter,
                    Title = a.Title,
                    Description = a.Description,
                    DishId = a.Dish.Id
                }).ToListAsync();
        }

        public async Task<AllergenDto?> GetAsync(int id)
        {
            return await Allergens.Where(a => a.Id == id).Select(a => new AllergenDto
            {
                Id = a.Id,
                Letter = a.Letter,
                Title = a.Title,
                Description = a.Description,
                DishId = a.Dish.Id
            }).FirstOrDefaultAsync();
        }

        public async Task<AllergenDto> InsertAsync(AllergenDto allergenDto)
        {
            var allergen = new Allergen
            {
                Id = allergenDto.Id,
                Letter = allergenDto.Letter,
                Title = allergenDto.Title,
                Description = allergenDto.Description,
                DishId = allergenDto.DishId
            };

            await Allergens.AddAsync(allergen);
            await context.SaveChangesAsync();
            allergenDto.Id = allergen.Id;
            return allergenDto;
        }

        public async Task<AllergenDto> UpdateAsync(AllergenDto allergenDto)
        {
            var allergen = new Allergen
            {
                Id = allergenDto.Id,
                Letter = allergenDto.Letter,
                Title = allergenDto.Title,
                Description = allergenDto.Description
            };

            var table = Allergens.Attach(allergen);
            table.State = EntityState.Modified;
            await context.SaveChangesAsync();
            allergenDto.Id = allergen.Id;
            return allergenDto;
        }

        public async Task<AllergenDto?> DeleteAsync(int id)
        {
            Allergen? allergen = await Allergens.FindAsync(id);
            if (allergen == null)
                return null;

            Allergens.Remove(allergen);
            await context.SaveChangesAsync();

            return new AllergenDto
            {
                Id = allergen.Id,
                Letter = allergen.Letter,
                Title = allergen.Title,
                Description = allergen.Description,
                DishId = allergen.Dish.Id
            };
        }
    }
}
