
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
            if (string.IsNullOrEmpty(name)) return new List<Venda>();

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
            var allProducts = await _repository.Get().ToListAsync();

            double mediaDeVendaPorDia = allProducts.GroupBy(p => p.DataVenda.Date)
                                                .Average(g => g.Sum(p => p.QuantidadeVendido));

            var produtoMaisVendidoGroup = allProducts.GroupBy(p => p.Nome)
                                                     .OrderByDescending(g => g.Sum(p => p.QuantidadeVendido))
                                                     .FirstOrDefault();

            var produtosResumo = await _repository.Get().GroupBy(v => v.Nome)
                                                  .Select(v => new ProdutoResumoDto
                                                  {
                                                      Nome = v.Key,
                                                      TotalDaVenda = v.Sum(v => v.TotalDaVenda),
                                                      QuantidadeTotalVendida = v.Sum(v => v.QuantidadeVendido)
                                                  }).ToListAsync();

            double totalDeTodasAsVendas = allProducts.Sum(v => v.TotalDaVenda);

            double? totalDoMaisVendido = produtoMaisVendidoGroup?.Sum(p => p.QuantidadeVendido * p.Preco);

            string produtoMaisVendido = produtoMaisVendidoGroup?.Key;

            return new RemusoVendasDto
            {
                ProdutosResumo = produtosResumo,
                
                MediaDeVendaPorDia = mediaDeVendaPorDia,
                TotalDeTodasAsVendas = totalDeTodasAsVendas,
                
                ProdutoMaisVendido = produtoMaisVendido,
                TotalDoMaisVendido = totalDoMaisVendido ?? 0
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
