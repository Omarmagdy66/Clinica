using Cls.Api.Dto;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration configuration)
        {
            this._config = configuration;
        }
        
        public string GenerateJSONWebToken<T>(T user, string role) where T : class
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> userClaims = new List<Claim>();

            // Assuming the user class has an Id and UserName properties, we can access them dynamically.
            // You may need to adapt this based on your actual implementation of Admin, Patient, Doctor, etc.
            var userIdProperty = typeof(T).GetProperty("Id");
            var userNameProperty = typeof(T).GetProperty("Name");

            if (userIdProperty != null && userNameProperty != null)
            {
                userClaims.Add(new Claim(ClaimTypes.NameIdentifier, userIdProperty.GetValue(user).ToString()));
                userClaims.Add(new Claim(ClaimTypes.Name, userNameProperty.GetValue(user).ToString()));
            }

            userClaims.Add(new Claim(ClaimTypes.Role, role));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
