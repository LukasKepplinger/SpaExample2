using Bestellservice4.Server;
using Bestellservice4.Server.Hubs;
using Bestellservice4.Services;
using Bestellservice4.Services.IServices;
using Bestellservice4.Services.Models;
using Bestellservice4.Services.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Bestellservice4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<IDishService, DishService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Bearer JWT Authorization header",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });

            builder.Services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
                {
                    options.IdentityResources["openid"].UserClaims.Add("role");
                    options.ApiResources.Single().UserClaims.Add("role");
                });
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

            builder.Services.AddAuthentication()
                .AddIdentityServerJwt();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddEndpointDefinitions(typeof(Dish));
            builder.Services.AddSignalR();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();
            app.UseEndpointDefinitions();
            app.MapHub<NotificationHub>("/notification");
            app.MapFallbackToFile("index.html");


            var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            CreateRoles(serviceProvider).Wait();
            CreateDefaultUsers(serviceProvider).Wait();

            app.Run();
        }




        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            bool adminRoleExists = await roleManager.RoleExistsAsync("Admin");
            if (!adminRoleExists)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Company"));
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }
        }

        public static async Task CreateDefaultUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var newUser = new ApplicationUser();
            var user = new ApplicationUser();

            user = await userManager.FindByEmailAsync("admin.1@gmail.com");
            if (user == null)
            {
                newUser = new ApplicationUser()
                {
                    Email = "admin.1@gmail.com",
                    UserName = "admin.1@gmail.com"
                };
                await userManager.CreateAsync(newUser, "Abc.123");
            }
            user = await userManager.FindByEmailAsync("admin.1@gmail.com");
            await userManager.AddToRoleAsync(user, "Admin");


            user = await userManager.FindByEmailAsync("company.1@gmail.com");
            if (user == null)
            {
                newUser = new ApplicationUser()
                {
                    Email = "company.1@gmail.com",
                    UserName = "company.1@gmail.com"
                };
                await userManager.CreateAsync(newUser, "Abc.123");
            }
            user = await userManager.FindByEmailAsync("company.1@gmail.com");
            await userManager.AddToRoleAsync(user, "Company");


            user = await userManager.FindByEmailAsync("customer.1@gmail.com");
            if (user == null)
            {
                newUser = new ApplicationUser()
                {
                    Email = "customer.1@gmail.com",
                    UserName = "customer.1@gmail.com"
                };
                await userManager.CreateAsync(newUser, "Abc.123");
            }
            user = await userManager.FindByEmailAsync("customer.1@gmail.com");
            await userManager.AddToRoleAsync(user, "Customer");
        }
    }
}