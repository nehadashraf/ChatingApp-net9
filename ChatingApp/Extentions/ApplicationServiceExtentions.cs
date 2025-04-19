using ChatingApp.Data;
using ChatingApp.Interfaces;
using ChatingApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatingApp.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
        {

            services.AddControllers();
            services.AddDbContext<Context>(
                options => options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
            );
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
