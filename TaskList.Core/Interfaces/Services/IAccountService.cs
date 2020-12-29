using System;
using System.Collections.Generic;
using System.Text;

namespace TaskList.Core.Interfaces.Services
{
    public interface IAccountService
    {
        int Login(string userName, string password);

        void Logout();

        string GenerateToken(string userName, int userId);
    }
}
