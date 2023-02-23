using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Domain;

namespace Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces
{
    public interface IAuthService
    {
        Task<int> Register(User user, string Password);
        Task<string> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}