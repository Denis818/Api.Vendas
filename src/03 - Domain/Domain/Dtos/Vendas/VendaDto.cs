namespace Domain.Models.Dto
{
    public class VendaDto
    {
        public string NomeProduto { get; set; }
        public double Preco { get; set; }
        public int QuantidadeVendido { get; set; }
    }

    public class VendaPorDiaDto
    {
        public DateTime Data { get; set; }
        public double TotalVendas { get; set; }
    }
}
