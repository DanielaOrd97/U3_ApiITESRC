using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using U3Api.Helpers;
using U3Api.Models.DTOs;
using U3Api.Models.Entities;
using U3Api.Models.Validators;
using U3Api.Repositories;

namespace U3Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        public Repository<Departamentos> Repo { get; }

        public LogInController(Repository<Departamentos> repo)
        {
                Repo = repo;
        }


        [HttpPost]
        public IActionResult LogIn(LogInDto dto)
        {

            LogInValidator validator = new();
            var resultados = validator.Validate(dto);

            if (resultados.IsValid)
            {
                var usuario = Authenticate(dto);

                if (usuario != null)
                {
                    dto.NombreDept = usuario.Nombre;
                    JwtTokenGenerator jwtToken = new();
                    return Ok(jwtToken.GetToken(dto));
                }

                return NotFound("Acceso denegado");
            }

            return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
            
        }

        private Departamentos Authenticate(LogInDto dto)
        {
            var pass = ConvertPasswordToSHA512(dto.Password);
            var usuario = Repo.GetAll().Where(x => x.Username.ToLower() == dto.Username.ToLower() && x.Password == pass).FirstOrDefault();

            if (usuario != null)
            {
                return usuario;
            }

            return null;
        }

        public static string ConvertPasswordToSHA512(string password)
        {
            using (var sha512 = SHA512.Create())
            {
                var arreglo = Encoding.UTF8.GetBytes(password);
                var hash = sha512.ComputeHash(arreglo);
                return Convert.ToHexString(hash).ToLower();
            }
        }
    }
}
