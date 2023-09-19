namespace Domain.Dtos.Vendas
{
    public class RemusoVendasDto
    {
        public double MediaDeVendaPorDia { get; set; }
        public string ProdutoMaisVendido { get; set; }
        public double TotalVendasHoje { get; set; }
        public double TotalDeTodasAsVendas { get; set; }
        public IEnumerable<ProdutoResumoDto> ProdutosResumo { get; set; }
    }

    public class ProdutoResumoDto
    {
        public string Nome { get; set; }
        public double TotalDaVenda { get; set; }
        public int QuantidadeTotalVendida { get; set; }
    }
}
