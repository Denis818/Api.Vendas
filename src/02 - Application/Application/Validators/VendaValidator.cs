﻿using Domain.Models.Dto;
using FluentValidation;

namespace Application.Validators
{
    public class VendaValidator : AbstractValidator<VendaDto>
    {
        public VendaValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("É obrigatório.")
                                .Length(3, 25).WithMessage("Deve ter entre 3 a 25 caracteres.");

            RuleFor(x => x.Preco).InclusiveBetween(0.01, 999)
                                      .WithMessage("Não pode ser menor que 0.1, e maior que 999.");

            RuleFor(x => x.QuantidadeVendido).InclusiveBetween(1, 999)
                                      .WithMessage("Não pode ser menor que 1, e maior que 999."); 
        }
    }
}
