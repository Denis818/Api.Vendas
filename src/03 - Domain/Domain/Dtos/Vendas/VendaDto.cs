using Domain.Models;
using Domain.Models.Dto;

namespace Domain.Dtos.Vendas
{
    public class VendaDto
    {
        public DateTime DataVenda { get; set; }
        public List<ProdutoDto> Produtos { get; set; }
    }
}
