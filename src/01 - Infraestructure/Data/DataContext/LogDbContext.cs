using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.DataContext
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
        {
        }

        public DbSet<LogAcesso> LogAcessos { get; set; }
    }
}
