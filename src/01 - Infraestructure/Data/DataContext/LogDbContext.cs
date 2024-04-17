using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.DataContext
{
    public class LogDbContext(DbContextOptions<LogDbContext> options) : DbContext(options)
    {
        public DbSet<LogVenda> LogsVendas { get; set; }
        public DbSet<LogRequest> LogsRequests { get; set; }
    }
}