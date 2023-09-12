namespace Domain.Dtos.Vendas
{
    public class RemusoVendasDto
    {
        public double MediaDeVendaPorDia { get; set; }
        public string ProdutoMaisVendido { get; set; }
        public double TotalDoMaisVendido { get; set; }
        public double TotalDeTodasAsVendas { get; set; }
    }
}
