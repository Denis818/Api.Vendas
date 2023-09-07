namespace Domain.Models.Dto
{
    public class ProdutoDto
    {
        public string Nome { get; set; }   
        public double Preco { get; set; }
        public DateTime DataVenda { get; set; }
        public int Quantidade { get; set; }
    }
}
