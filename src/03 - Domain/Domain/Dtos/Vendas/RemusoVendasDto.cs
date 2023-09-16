namespace Domain.Dtos.Vendas
{
    public class RemusoVendasDto
    {
        public double MediaDeVendaPorDia { get; set; }
        public string ProdutoMaisVendido { get; set; }
        public double TotalDoMaisVendido { get; set; }
        public double TotalDeTodasAsVendas { get; set; }
        public List<ProdutoResumoDto> ProdutosResumo { get; set; }
    }

    public class ProdutoResumoDto
    {
        public string Nome { get; set; }
        public double TotalDaVenda { get; set; }
        public int QuantidadeTotalVendida { get; set; }
    }
}
