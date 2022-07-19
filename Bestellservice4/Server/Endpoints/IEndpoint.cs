
namespace Bestellservice4.Server.Endpoints
{
    public interface IEndpoint
    {
        void DefineEndpoints(WebApplication app);
        void DefineServices(IServiceCollection services);
    }
}