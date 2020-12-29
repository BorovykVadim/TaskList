using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.Extensions.DependencyInjection;
using TaskList.Core.Models;

namespace TaskList.Data
{
    public class DatabaseInitializer
    {
        public static void Init(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = new User
            {
                UserName = "UserName"
            };

            var result = userManager.CreateAsync(user, "123qwe").GetAwaiter().GetResult();
        }
    }
}
