
using Microsoft.AspNetCore.Builder;

namespace Application.Configurations
{
    public static class CorsPolicyExtension
    {
        public static void UseCorsPolicy(this IApplicationBuilder app)
        {
            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
        }

    }
}
