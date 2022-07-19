using Bestellservice4.Services;
using Bestellservice4.Services.IServices;
using Bestellservice4.Services.Services;
using Bestellservice4.Shared.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;

namespace Bestellservice4.Server.Endpoints
{
    public class DishEndpoint : IEndpoint
    {

        public void DefineEndpoints(WebApplication app)
        {
            app.MapGet("/data/dishes/test", () => "the dishes endpoint is responding");
            app.MapGet("/data/dishes/all", GetAll);
            app.MapGet("/data/dishes/{id}", GetById);
            app.MapPost("/data/dishes/add", Add);
            app.MapPut("/data/dishes/update", Update);
            app.MapDelete("/data/dishes/{id}", Delete);
        }

        [Authorize(Roles = "Customer, Company, Admin")]
        internal async Task<List<DishDto>> GetAll(IDishService dishService)
        {
            return await dishService.GetAllAsync();
        }

        [ProducesResponseType(200, Type = typeof(DishDto))]
        [Authorize(Roles = "Customer, Company, Admin")]
        internal async Task<IResult> GetById(IDishService dishService, int? id)
        {
            if (id == null) return Results.NotFound();
            var dishDto = await dishService.GetAsync(id.Value);
            if (dishDto == null) return Results.NotFound();
            return Results.Ok(dishDto);
        }

        [Authorize(Roles = "Admin")]
        internal async Task<IResult> Add(IDishService dishService, DishDto dishDto)
        {
            dishDto.Created = DateTime.Now;
            if (MiniValidator.TryValidate(dishDto, out var errors))
            {
                await dishService.InsertAsync(dishDto);
                return Results.Ok(dishDto);
            }
            return Results.ValidationProblem(errors);
        }

        [Authorize(Roles = "Admin")]
        internal async Task<IResult> Update(IDishService dishService, DishDto? dishDto)
        {
            if (MiniValidator.TryValidate(dishDto, out var errors))
            {
                await dishService.UpdateAsync(dishDto);
                return Results.Ok(dishDto);
            }
            return Results.ValidationProblem(errors);
        }

        [ProducesResponseType(200, Type = typeof(DishDto))]
        [Authorize(Roles = "Admin")]
        internal async Task<IResult> Delete(IDishService dishService, int? id)
        {
            if (id == null) return Results.NotFound();
            var dishDto = await dishService.DeleteAsync(id.Value);
            if (dishDto == null) return Results.NotFound();
            return Results.Ok(dishDto);
        }

        public void DefineServices(IServiceCollection services)
        {
            services.AddScoped<IDishService, DishService>();
        }
    }
}
