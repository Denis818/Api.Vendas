﻿
using Api.Vendas.Utilities;
using Application.Constants;
using Application.Interfaces.Services;
using Application.Services.Base;
using Application.Utilities;
using Data.Repository;
using Domain.Dtos.Vendas;
using Domain.Interfaces.Repository;
using Domain.Models;
using Domain.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Services
{
    public class VendasServices : ServiceAppBase<Venda, VendaDto, IVendaRepository>, IVendasServices
    {
        public VendasServices(IServiceProvider service) : base(service)
        {
        }

        public async Task<PagedResult<Venda>> GetAllVendasAsync(int paginaAtual, int itensPorPagina)
        {
            var vendasQuery = _repository.Get();

            return await Pagination.PaginateResult(vendasQuery, paginaAtual, itensPorPagina);
        }

        public List<Venda> GetSalesByDate(DateTime? startDate, DateTime? endDate)
        {
            var query = _repository.Get();

            if (startDate.HasValue)
            {
                query = query.Where(v => v.DataVenda.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                // Adiciona um dia à endDate para incluir vendas que ocorreram durante todo o dia final do intervalo
                DateTime endOfDay = endDate.Value.Date.AddDays(1);
                query = query.Where(v => v.DataVenda.Date < endOfDay.Date);
            }

            return query.ToList();
        }

        public async Task<Venda> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<Venda>> FilterSalesByName(string name)
        {
            if (name.IsNullOrEmpty()) return await _repository.Get().ToListAsync();

            var lowerName = name.ToLower();

            return await _repository.Get(venda => venda.Nome.ToLower()
                                    .Contains(lowerName)).ToListAsync();
        }

        public async Task<List<VendasPorDiaDto>> GetGroupSalesDayAsync()
        {
            var allProducts = await _repository.Get().ToListAsync();

            var culture = new CultureInfo("pt-BR");

            var vendasPorDia = allProducts
                .GroupBy(p => p.DataVenda.DayOfWeek)
                .Select(g => new
                {
                    dia = culture.DateTimeFormat.GetDayName(g.Key),
                    total = g.Sum(p => p.TotalDaVenda),
                    order = (int)g.Key
                })
                .OrderBy(x => x.order)
                .ToList();

            return vendasPorDia.Select(x => new VendasPorDiaDto { Dia = x.dia, Total = x.total }).ToList();
        }

        public async Task<RemusoVendasDto> GetSalesSummaryAsync()
        {
            var hoje = DateTime.Now.Date;
            var inicioDaSemana = hoje.AddDays(-(int)DateTime.Now.DayOfWeek);
            var finalDaSemana = inicioDaSemana.AddDays(7);

            // Carrega todos os dados necessários em uma única consulta
            var allSales = await _repository.Get().ToListAsync();

            // Processa os dados na memória
            var produtoMaisVendidoDaSemana = allSales
                .Where(p => p.DataVenda.Date >= inicioDaSemana && p.DataVenda.Date < finalDaSemana)
                .GroupBy(p => p.Nome)
                .Select(g => new
                {
                    Produto = g.Key,
                    TotalVendas = g.Sum(p => p.QuantidadeVendido)
                })
                .OrderByDescending(p => p.TotalVendas)
                .FirstOrDefault();

            double mediaDeVendaPorDia = allSales
                .GroupBy(p => p.DataVenda.Date)
                .Average(g => g.Sum(p => p.QuantidadeVendido));

            double totalVendasHoje = allSales
                .Where(v => v.DataVenda.Date == hoje)
                .Sum(v => v.TotalDaVenda);

            var produtosResumo = allSales
                .GroupBy(v => v.Nome)
                .Select(v => new ProdutoResumoDto
                {
                    Nome = v.Key,
                    TotalDaVenda = v.Sum(v => v.TotalDaVenda),
                    QuantidadeTotalVendida = v.Sum(v => v.QuantidadeVendido)
                });

            var totalDeTodasAsVendas = allSales.Sum(v => v.TotalDaVenda);

            return new RemusoVendasDto
            {
                ProdutosResumo = produtosResumo,
                TotalVendasHoje = totalVendasHoje,
                MediaDeVendaPorDia = mediaDeVendaPorDia,
                TotalDeTodasAsVendas = totalDeTodasAsVendas,
                ProdutoMaisVendido = produtoMaisVendidoDaSemana?.Produto
            };
        }

        public async Task<Venda> InsertAsync(VendaDto vendaDto)
        {
            if (Validator(vendaDto)) return null;

            var venda = MapToModel(vendaDto);

            venda.TotalDaVenda = venda.QuantidadeVendido * venda.Preco;

            venda.DataVenda = DateTime.Now;

            await _repository.InsertAsync(venda);

            if (!await _repository.SaveChangesAsync())
            {
                Notificar(EnumTipoNotificacao.ServerError, ErrorMessages.InsertError);
                return null;
            }

            await InsertLog(_context.User.Identity.Name, venda, "Criação de Venda");

            return venda;
        }

        public async Task<Venda> UpdateAsync(int id, VendaDto vendaDto)
        {
            var venda = await _repository.GetByIdAsync(id);

            if (venda == null)
            {
                Notificar(EnumTipoNotificacao.ClientError, ErrorMessages.NotFoundById + id);
                return null;
            }

            if (Validator(vendaDto)) return null;

            MapDtoToModel(vendaDto, venda);

            venda.TotalDaVenda = venda.QuantidadeVendido * venda.Preco;

            _repository.Update(venda);

            if (!await _repository.SaveChangesAsync())
            {
                Notificar(EnumTipoNotificacao.ServerError, ErrorMessages.UpdateError);
                return null;
            }

            return venda;
        }

        public async Task DeleteAsync(int id)
        {
            var venda = await _repository.GetByIdAsync(id);

            if (venda == null)
            {
                Notificar(EnumTipoNotificacao.ClientError, ErrorMessages.NotFoundById + id);
                return;
            }

            _repository.Delete(venda);

            if (!await _repository.SaveChangesAsync())
            {
                Notificar(EnumTipoNotificacao.ServerError, ErrorMessages.DeleteError);
                return;
            }

            Notificar(EnumTipoNotificacao.Informacao, "Registro Deletado");
        }

        public async Task DeleteRangerAsync(int[] ids)
        {
            var vendas = _repository.Get(venda => ids.Contains(venda.Id)).ToArray();

            if (vendas.IsNullOrEmpty())
            {
                Notificar(EnumTipoNotificacao.ClientError, ErrorMessages.NotFoundByIds + string.Join(", ", ids));
                return;
            }

            var idsNaoEncontrados = ids.Except(vendas.Select(evento => evento.Id));

            if (idsNaoEncontrados.Any())
            {
                string idsNotFound = $"{string.Join(", ", idsNaoEncontrados)}. Encontrados foram deletados";

                Notificar(EnumTipoNotificacao.Informacao, ErrorMessages.NotFoundByIds + idsNotFound);
            }

            _repository.DeleteRange(vendas);

            if (!await _repository.SaveChangesAsync())
            {
                Notificar(EnumTipoNotificacao.ServerError, ErrorMessages.DeleteError);
                return;
            }

            Notificar(EnumTipoNotificacao.Informacao, "Registros Deletados");
        }
    }
}
