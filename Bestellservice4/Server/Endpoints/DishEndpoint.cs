using Bestellservice4.Services;
using Bestellservice4.Services.IServices;
using Bestellservice4.Services.Services;
using Bestellservice4.Shared.Helper;
using Bestellservice4.Shared.Params;
using Bestellservice4.Shared.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Newtonsoft.Json;

namespace Bestellservice4.Server.Endpoints
{
    public class DishEndpoint : IEndpoint
    {

        public void DefineEndpoints(WebApplication app)
        {
            app.MapGet("/data/dishes/test", () => "the dishes endpoint is responding");
            app.MapGet("/data/dishes/all", GetAll);
            app.MapGet("/data/dishes/page", /*(PageParams pageParams) => */GetPage);
            app.MapGet("/data/dishes/{id}", GetById);
            app.MapPost("/data/dishes/add", Add);
            app.MapPut("/data/dishes/update", Update);
            app.MapDelete("/data/dishes/{id}", Delete);
        }


        [Authorize(Roles = "Admin")]
        internal async Task<IResult> GetAll(IDishService dishService)
        {
            var dishes = await dishService.GetAllAsync();
            if (dishes == null)
                return Results.NotFound();
            return Results.Ok(dishes);
        }


        [ProducesResponseType(200, Type = typeof(PageOf<DishDto>))]
        //[Authorize(Roles = "Customer, Company, Admin")]
        internal async Task<IResult> GetPage(IDishService dishService, HttpResponse response,
            [FromQuery(Name = "page-current")] int currentPage,
            [FromQuery(Name = "page-size")] int pageSize)
        {
            PageParams pageParams = new PageParams()
            {
                CurrentPage = currentPage,
                PageSize = pageSize
            };

            if (MiniValidator.TryValidate(pageParams, out var errors))
            {
                var dishPage = await dishService.GetPageAsync(pageParams);
                if (dishPage == null)
                    return Results.NotFound();

                response.Headers.Add("Page-Metadata", JsonConvert.SerializeObject(dishPage.PageMetaData));
                return Results.Ok(dishPage);
            }
            return Results.ValidationProblem(errors);
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
