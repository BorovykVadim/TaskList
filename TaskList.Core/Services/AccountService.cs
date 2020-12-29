using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskList.Core.Interfaces.Services;
using TaskList.Core.Models;
using TaskList.Core.Options;

namespace TaskList.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IOptions<AuthOptions> options;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IOptions<AuthOptions> options)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.options = options;
        }

        public int Login(string userName, string password)
        {
            var user = userManager.FindByNameAsync(userName).GetAwaiter().GetResult();

            if (user == null)
            {
                user = new User { UserName = userName };

                var createResul = userManager.CreateAsync(user, password).GetAwaiter().GetResult();
            }

            var existingUser = userManager.FindByNameAsync(userName).GetAwaiter().GetResult();

            var signInResult = signInManager.PasswordSignInAsync(existingUser, password, false, false).GetAwaiter().GetResult();

            if (signInResult.Succeeded)
            {
                return existingUser.Id;
            }

            return -1;

        }

        public void Logout()
        {
            signInManager.SignOutAsync();
        }

        public string GenerateToken(string userName, int userId)
        {
            var authParams = options.Value;

            var secretKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, userName)
            };

            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifeTime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
