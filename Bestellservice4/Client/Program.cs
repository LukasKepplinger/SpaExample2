using Bestellservice4.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;

namespace Bestellservice4.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjcyODgwQDMyMzAyZTMyMmUzMEZWa3V4a3E2WmZNM3VIaGQrL2xiNzJYNUQrZVZVY0RjOHdyZ0RmNkxSSjg9");

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddHttpClient("Bestellservice4.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Bestellservice4.ServerAPI"));

            builder.Services.AddApiAuthorization()
                 .AddAccountClaimsPrincipalFactory<CustomUserFactory>();

            builder.Services.AddSyncfusionBlazor();
            await builder.Build().RunAsync();
        }
    }
}