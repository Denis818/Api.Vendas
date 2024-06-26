﻿using Domain.Converters.DatesTimes;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class LogVenda
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        [JsonConverter(typeof(TimeFormatConverter))]
        public DateTime DataAcesso { get; set; }
        public string Acao { get; set; }


        public int VendaId { get; set; }
        public string NomeProduto { get; set; }
        public double PrecoProduto { get; set; }
        public int QuantidadeVendido { get; set; }
    }
}
