using Api.Vendas.Extensios;
using Application.Configurations;
using Application.Configurations.Extensions;
using Data.Configurations.Extensions;
using Domain.Interfaces.UserMain;
using ProEventos.API.Configuration.Middleware;

namespace Api.Vendas
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddConectionsString(Configuration);
            services.AddApiDependencyServices(Configuration);
            services.AddSwaggerAuthorizationJWT(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeedUser seedUser)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            seedUser.CreateSeedUser();

            app.UseCorsPolicy();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<MiddlewareException>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(opt => opt.MapControllers());
        }
    }
}