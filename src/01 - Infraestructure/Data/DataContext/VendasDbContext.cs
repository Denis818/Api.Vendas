using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.DataContext.Context
{
    public partial class VendasDbContext(DbContextOptions<VendasDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Venda> Vendas { get; set; }
    }
}
