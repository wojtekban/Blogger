using Microsoft.OpenApi.Models;
using Application.Interfaces;
using Application.Services;
using Application.Mappings;
using Domain.Interfaces;
using Infrastructure.Repositories;
using WebAPI.Installer;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.Edm;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.InstallServicesInAssembly(Configuration);
       // services.AddControllers().AddOData(opt => opt.Count().Filter().OrderBy().Expand().SetMaxTop(100)
      //                          .AddRouteComponents("odata", GetEdmModel()));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<Application.Dto.PostDto>("Posts");
        return builder.GetEdmModel();
    }
}