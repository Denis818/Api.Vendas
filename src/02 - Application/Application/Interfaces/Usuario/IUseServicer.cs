﻿using Api.Vendas.Utilities;
using Domain.Enumeradores;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Services.Usuario
{
    public interface IUserService
    {
        public string Name { get; }

        bool PossuiPermissao(params EnumPermissoes[] permissoesParaValidar);
        Task<string> RemovePermissionFromUser(string userEmail, string permisson);
        Task<string> AddPermissionToUser(string userEmail, string permisson);
    }
}
