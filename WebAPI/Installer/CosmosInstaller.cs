
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using Cosmonaut;
using Domain.Entities.Cosmos;
using Microsoft.Azure.Documents.Client;

namespace WebAPI.Installer
{
    public class CosmosInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            var cosmosStoreSettings = new CosmosStoreSettings(
                Configuration["CosmosSettings:DatabaseName"],
                Configuration["CosmosSettings:AccountUri"],
                Configuration["CosmosSettings:AccountKey"],
                new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp });

            services.AddCosmosStore<CosmosPost>(cosmosStoreSettings);
        }
    }
}
