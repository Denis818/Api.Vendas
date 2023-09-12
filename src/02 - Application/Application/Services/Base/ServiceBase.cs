using Application.Interfaces.Utility;
using Application.Utilities;
using AutoMapper;
using Domain.Interfaces.Repository.Base;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Base
{
    public abstract class ServiceAppBase<TEntity, TEntityDto, TIRepository>
      where TEntity : class, new()
      where TIRepository : class, IRepositoryBase<TEntity>
    {
        protected readonly IMapper _mapper;
        protected readonly TIRepository _repository;
        protected readonly INotificador _notificador;
        protected readonly IValidator<TEntityDto> _validator;

        protected ServiceAppBase(IServiceProvider service)
        {
            _mapper = service.GetRequiredService<IMapper>();
            _repository = service.GetRequiredService<TIRepository>();
            _notificador = service.GetRequiredService<INotificador>();
            _validator = service.GetRequiredService<IValidator<TEntityDto>>();
        }

        public void MapDtoToModel(TEntityDto entityDto, TEntity entity)
          => _mapper.Map(entityDto, entity);

        public TEntityDto MapToDto(TEntity entity)
            => _mapper.Map<TEntityDto>(entity);

        public TEntity MapToModel(TEntityDto entityDto)
            => _mapper.Map<TEntity>(entityDto);

        public IEnumerable<TEntityDto> MapToListDto(IEnumerable<TEntity> entityDto)
            => _mapper.Map<IEnumerable<TEntityDto>>(entityDto);

        public void Notificar(EnumTipoNotificacao tipo, string message)
            => _notificador.Add(new Notificacao(tipo, message));

        public bool Validator(TEntityDto entityDto)
        {
            ValidationResult results = _validator.Validate(entityDto);

            if (!results.IsValid)
            {
                var groupedFailures = results.Errors
                                             .GroupBy(failure => failure.PropertyName)
                                             .Select(group => new {
                                                 PropertyName = group.Key,
                                                 Errors = string.Join(" ", group.Select(err => err.ErrorMessage))
                                             });

                foreach (var failure in groupedFailures)
                {
                    Notificar(EnumTipoNotificacao.Informacao, $"{failure.PropertyName}: {failure.Errors}");
                }

                return true;
            }

            return false;
        }
    }
}
