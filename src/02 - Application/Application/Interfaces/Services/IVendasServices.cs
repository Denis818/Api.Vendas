using Api.Vendas.Utilities;
using Domain.Dtos.Vendas;
using Domain.Models;
using Domain.Models.Dto;

namespace Application.Interfaces.Services
{
    public interface IVendasServices
    {
    
        Task<PagedResult<Venda>> GetAllVendasAsync(int paginaAtual, int itensPorPagina);
        Task<List<VendasPorDiaDto>> GetGroupSalesDayAsync();
        List<Venda> GetSalesByDate(DateTime? startDate, DateTime? endDate);
        Task<RemusoVendasDto> GetSalesSummaryAsync();   
        Task<List<Venda>> FilterSalesByName(string name);
        Task<Venda> GetByIdAsync(int id);
        Task<Venda> InsertAsync(VendaDto vendaDto);
        Task<Venda> UpdateAsync(int id, VendaDto vendaDto);
        Task DeleteAsync(int id);
        Task DeleteRangerAsync(int[] ids);
    }
}