﻿using blogPessoal.Model;

namespace blogpessoal.Security
{
    public interface IAuthService
    {
        Task<UserLogin?> Autenticar(UserLogin userLogin);
    }
}