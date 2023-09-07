namespace Domain.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public DateTime DataVenda { get; set; }
        public List<Produto> Produtos { get; set; }
    }

    public class VendaPorDia
    {
        public DateTime Data { get; set; }
        public double TotalVendas { get; set; }
    }

}
