﻿using Application.Interfaces.Services;
using Domain.Converters.DatesTimes;
using Domain.Interfaces.Repository;
using Domain.Models;

namespace Application.Services.LogApplication
{
    public class LogVendaServices(ILogVendaRepository logAcesso) : ILogVendaServices
    {
        protected readonly ILogVendaRepository _logAcesso = logAcesso;

        public async Task InsertLog(string userName, Venda venda, string acao)
        {
            var log = new LogVenda
            {
                UserName = userName,
                DataAcesso = DateTimeZoneProvider.GetBrasiliaTimeZone(DateTime.UtcNow),

                VendaId = venda.Id,
                NomeProduto = venda.Nome,
                PrecoProduto = venda.Preco,
                QuantidadeVendido = venda.QuantidadeVendido,

                Acao = acao
            };

            await _logAcesso.InsertAsync(log);
            await _logAcesso.SaveChangesAsync();
        }
    }
}
