using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_U3.Repositories;
using U3Api.Models.DTOs;
using U3Api.Models.Entities;

namespace Proyecto_U3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosController : ControllerBase
    {
        public Repository<Departamentos> Repository { get; }

        public DepartamentosController(Repository<Departamentos> repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public IActionResult GetAllDepartamentos()
        {
            var deptos = Repository.GetAll().
                            OrderBy(x => x.Id).
                            Select(x => new DepartamentoDTO
                            {
                                Id = x.Id,
                                NombreDepartamento = x.Nombre,
                                Username = x.Username,
                                Password = x.Password,
                                IdSuperior = x.IdSuperior
                            });
            return Ok(deptos);
        }
    }
}
