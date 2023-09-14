using Application.Configurations.Extensions;
using Data.Configurations.Extensions;
using Api.Vendas.Converters;
using Application.Configurations;
using ProEventos.API.Configuration.Middleware;
using Api.Vendas.Extensios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
}); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddConectionsString(builder.Configuration);
builder.Services.AddApiDependencyServices(builder.Configuration);
builder.Services.AddSwaggerAuthorizationJWT(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCorsPolicy();

app.UseHttpsRedirection();

app.UseRouting();

app.UseMiddleware<MiddlewareException>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
