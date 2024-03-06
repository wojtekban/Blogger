
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI.Installer;

namespace WebAPI.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<BloggerContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BloggerCS")));
        }
    }
}