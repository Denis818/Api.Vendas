using Domain.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Data.DataContext
{
    public class LogDbContext(DbContextOptions<LogDbContext> options) : DbContext(options)
    {
        public DbSet<LogVenda> LogVendas { get; set; }
        public DbSet<LogError> LogErrors { get; set; }
        public DbSet<LogRequest> LogRequests { get; set; }
    }
}