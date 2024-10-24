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

        public string GenerateJSONWebToken<T>(T user, string roleId, string roleName) where T : class
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> userClaims = new List<Claim>();

            // Access user properties dynamically
            var userIdProperty = typeof(T).GetProperty("Id");
            var userNameProperty = typeof(T).GetProperty("Name");

            if (userIdProperty != null && userNameProperty != null)
            {
                userClaims.Add(new Claim("id", userIdProperty.GetValue(user).ToString())); // Custom claim for ID
                userClaims.Add(new Claim("name", userNameProperty.GetValue(user).ToString())); // Custom claim for Name
            }

            // Add role ID and role name as claims
            userClaims.Add(new Claim("roleid", roleId)); // Custom claim for RoleId
            userClaims.Add(new Claim("role", roleName)); // Custom claim for RoleName
            userClaims.Add(new Claim(ClaimTypes.Role, roleId));

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //public string GenerateJSONWebToken<T>(T user, string roleId, string roleName) where T : class
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    List<Claim> userClaims = new List<Claim>();

        //    var userIdProperty = typeof(T).GetProperty("Id");
        //    var userNameProperty = typeof(T).GetProperty("Name");

        //    if (userIdProperty != null && userNameProperty != null)
        //    {
        //        userClaims.Add(new Claim("id", userIdProperty.GetValue(user).ToString())); // Custom claim for ID
        //        userClaims.Add(new Claim("name", userNameProperty.GetValue(user).ToString())); // Custom claim for Name
        //    }

        //    // Add roleid and rolename as claims
        //    userClaims.Add(new Claim("roleid", roleId));
        //    userClaims.Add(new Claim("rolename", roleName));

        //    // Add ClaimTypes.Role claim to map it with "Roles" authorization
        //    userClaims.Add(new Claim(ClaimTypes.Role, roleId)); // Using roleId as the role

        //    userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        //    var token = new JwtSecurityToken(
        //        issuer: _config["Jwt:Issuer"],
        //        audience: _config["Jwt:Audience"],
        //        claims: userClaims,
        //        expires: DateTime.Now.AddMinutes(120),
        //        signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}




    }
}
