using Bestellservice4.Services.IServices;
using Bestellservice4.Services.Services;
using Bestellservice4.Shared.Transfer;
using Microsoft.AspNetCore.Authorization;
using MiniValidation;

namespace Bestellservice4.Server.Endpoints
{
    public class AllergenEndpoint : IEndpoint
    {
        public void DefineEndpoints(WebApplication app)
        {
            app.MapGet("/data/allergen/test", () => "the allergens endpoint is responding");
            app.MapGet("/data/allergen/all", GetAll);
            app.MapGet("/data/allergen/{dishId}", GetForDish);
            app.MapGet("/data/allergen/{id}", GetById);
            app.MapPost("/data/allergen/add", Add);
            app.MapPut("/data/allergen/update", Update);
            app.MapDelete("/data/allergen/{id}", Delete);
        }

        //[Authorize(Roles = "Admin")]
        internal async Task<IResult> GetAll(IAllergenService allergenService)
        {
            var allergens = await allergenService.GetAllAsync();
            if (!allergens.Any())
                return Results.NoContent();
            return Results.Ok(allergens);
        }

        //[Authorize(Roles = "Admin")]
        internal async Task<IResult> GetForDish(IAllergenService allergenService, int? dishId)
        {
            if(dishId == null)
                return Results.NotFound();
            var allergens = await allergenService.GetForDishAsync(dishId.Value);
            if (!allergens.Any())
                return Results.NoContent();
            return Results.Ok(allergens);
        }

        //[Authorize(Roles = "Admin")]
        internal async Task<IResult> GetById(IAllergenService allergenService, int? id)
        {
            if (id == null) return Results.NotFound();
            var allergen = await allergenService.GetAsync(id.Value);
            if (allergen == null) return Results.NotFound();
            return Results.Ok(allergen);
        }

        //[Authorize(Roles = "Admin")]
        internal async Task<IResult> Add(IAllergenService allergenService, AllergenDto allergenDto)
        {
            if (MiniValidator.TryValidate(allergenDto, out var errors))
            {
                await allergenService.InsertAsync(allergenDto);
                return Results.Ok(allergenDto);
            }
            return Results.ValidationProblem(errors);
        }

        //[Authorize(Roles = "Admin")]
        internal async Task<IResult> Update(IAllergenService allergenService, AllergenDto allergenDto)
        {
            if (MiniValidator.TryValidate(allergenDto, out var errors))
            {
                await allergenService.UpdateAsync(allergenDto);
                return Results.Ok(allergenDto);
            }
            return Results.ValidationProblem(errors);
        }

        //[Authorize(Roles = "Admin")]
        internal async Task<IResult> Delete(IAllergenService allergenService, int? id)
        {
            if (id == null) return Results.NotFound();
            var allergen = await allergenService.DeleteAsync(id.Value);
            if (allergen == null) return Results.NotFound();
            return Results.Ok(allergen);
        }

        public void DefineServices(IServiceCollection services)
        {
            services.AddScoped<IAllergenService, AllergenService>();
        }
    }
}
