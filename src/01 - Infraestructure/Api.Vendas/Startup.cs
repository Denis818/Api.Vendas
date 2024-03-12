using Api.Vendas.Extensios.Swagger;
using Api.Vendas.FiltersControllers;
using Application.Configurations.Extensions.DependencyManagers;
using Application.Configurations.UserMain;
using Data.Configurations.Extensions;
using Microsoft.AspNetCore.Localization;
using ProEventos.API.Configuration.Middleware;
using System.Globalization;

namespace Api.Vendas
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(LogRequestFilter));
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddConectionsString(Configuration);
            services.AddApiDependencyServices(Configuration);
            services.AddSwaggerAuthorizationJWT();
        }

        public void Configure(IApplicationBuilder app, IServiceProvider services)
        {
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = [new CultureInfo("pt-BR")],
                SupportedUICultures = [new CultureInfo("pt-BR")]
            });

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCorsPolicy();

            app.UseHttpsRedirection();

            app.UseRouting();

            services.ConfigurarBancoDados();

            app.UseMiddleware<MiddlewareException>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(options =>
            {
                options.MapControllers();
                options.MapGet("/{*path}", async context =>
                {
                    context.Response.Redirect("/swagger");
                    await Task.CompletedTask;
                });
            });
        }
    }
}