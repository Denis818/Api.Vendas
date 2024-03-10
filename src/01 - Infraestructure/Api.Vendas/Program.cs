namespace Api.Vendas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
                    webBuilder.UseStartup<Startup>();
                              //.UseUrls($"http://0.0.0.0:{port}");
                });
    }
}