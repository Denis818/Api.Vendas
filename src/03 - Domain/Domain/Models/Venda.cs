using Domain.Converters;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }

        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime DataVenda { get; set; }
        public int QuantidadeVendido { get; set; }
        public double TotalDaVenda { get; set; }
    }
}
