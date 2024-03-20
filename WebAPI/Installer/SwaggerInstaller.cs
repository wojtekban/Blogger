using Microsoft.OpenApi.Models;
using WebAPI.Installer;
using OData.Swagger.Services;

namespace WebAPI.Installer;

public class SwaggerInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration Configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
           
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });

        services.AddOdataSwaggerSupport();
    }
}