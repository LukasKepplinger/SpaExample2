

using Bestellservice4.Server;
using Bestellservice4.Server.Endpoints;

namespace Bestellservice4.Server
{
    [Obsolete]
    public static class EndpointsFinder
    {
        public static void AddEndpointDefinitions(this IServiceCollection services, params Type[] scanMarkers)
        {
            var endpointDefinitions = new List<IEndpoint>();

            //foreach (var marker in scanMarkers)
            //{
            //    endpointDefinitions.AddRange(
            //        marker.Assembly.ExportedTypes
            //        .Where(x => typeof(IEndpointDefinition).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            //        .Select(Activator.CreateInstance).Cast<IEndpointDefinition>()
            //        );
            //}
            
            endpointDefinitions.Add(new DishEndpoint());
            endpointDefinitions.Add(new AllergenEndpoint());


            foreach (var endpointDefinition in endpointDefinitions)
            {
                endpointDefinition.DefineServices(services);
            }

            services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndpoint>);
        }

        public static void UseEndpointDefinitions(this WebApplication app)
        {
            var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpoint>>();

            foreach (var endpointDefinition in definitions)
            {
                endpointDefinition.DefineEndpoints(app);
            }
        }
    }
}
