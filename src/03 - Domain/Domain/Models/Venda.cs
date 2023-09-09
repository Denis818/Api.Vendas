namespace Domain.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public string NomeProduto { get; set; }
        public double Preco { get; set; }
        public DateTime DataVenda { get; set; }
        public int QuantidadeVendido { get; set; }
        public double TotalDaVenda { get; set; }
    }
}
