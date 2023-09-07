using Data.Repository.Base;
using Domain.Interfaces.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class VendaRepository : RepositoryBase<Venda>, IVendaRepository
    {
        public VendaRepository(IServiceProvider service) : base(service)
        {

        }
        public async Task<List<VendaPorDia>> GetVendasPorDia()
        {
            var vendasAgrupadas = await Get()
                .Include(v => v.Produtos)
                .SelectMany(v => v.Produtos, (venda, produto) => new
                {
                    DataVenda = venda.DataVenda.Date,
                    TotalVendaProduto = produto.Preco * produto.Quantidade
                })
                .GroupBy(v => v.DataVenda.Date)
                .Select(g => new VendaPorDia
                {
                    Data = g.Key,
                    TotalVendas = g.Sum(v => v.TotalVendaProduto)
                })
                .ToListAsync();

            return vendasAgrupadas;
        }

    }
}
