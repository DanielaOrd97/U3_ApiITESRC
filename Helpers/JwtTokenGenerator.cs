using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using U3Api.Models.DTOs;

namespace U3Api.Helpers
{
    public class JwtTokenGenerator
    {
        //private readonly IConfiguration _config;

        //public JwtTokenGenerator(IConfiguration config)
        //{
        //    _config = config;
        //}
        public string GetToken(LogInDto dto)
        {
            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
            //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.Sha512);

            List<Claim> Claims = new();

            Claims.Add(new Claim(ClaimTypes.Role, dto.NombreDept));
            Claims.Add(new Claim(ClaimTypes.Email, dto.Username));
            //Claims.Add(new Claim(JwtRegisteredClaimNames.Iss, _config["Jwt:Issuer"]));
            Claims.Add(new Claim(JwtRegisteredClaimNames.Iss, "itesrcU3"));
            Claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "pruebaU3"));
            //Claims.Add(new Claim(JwtRegisteredClaimNames.Aud, _config["Jwt:Audience"]));
            Claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()));
            Claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(5).ToString())); //aumentar

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            var token = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(Claims),
                //Issuer = _config["Jwt:Issuer"],
                Issuer = "itesrcU3",
                //Audience = _config["Jwt:Audience"],
                Audience = "pruebaU3",
                IssuedAt = DateTime.Now,
                Expires = DateTime.UtcNow.AddMinutes(5),
                NotBefore = DateTime.UtcNow.AddMinutes(-1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("oWgVSB6azKciOUdwnRSMxKPyccYXiVp0qP0svFWemCQRK45kkbf3rqHbykHHYntKYyMxjKFJia9n7ZbKiC380uFSBuSuhzRd8IhY")), SecurityAlgorithms.HmacSha512)
            };

            return handler.CreateEncodedJwt(token);
        }
    }
}
