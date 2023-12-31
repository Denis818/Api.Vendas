using Domain.Models;

namespace Application.Interfaces.Services
{
    public interface ILogVendaServices
    {
        Task InsertLog(string userName, Venda venda, string acao);
    }
}