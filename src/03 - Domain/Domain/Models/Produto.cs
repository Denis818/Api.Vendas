﻿namespace Domain.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }   
        public double Preco { get; set; }
        public DateTime DataVenda { get; set; }
        public int Quantidade { get; set; }
    }
}
