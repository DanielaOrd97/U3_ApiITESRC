using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U3Api.Models.DTOs;
using U3Api.Models.Entities;
using U3Api.Models.Validators;
using U3Api.Repositories;

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

        [HttpPost]
        public IActionResult Post(DepartamentoDTO dto)
        {
            DepartamentoValidator validator = new();

            var resultados = validator.Validate(dto);

            if (resultados.IsValid)
            {
                Departamentos entity = new()
                {
                    Id = 0,
                    Nombre = dto.NombreDepartamento,
                    Username = dto.Username,
                    Password = dto.Password,
                    IdSuperior = dto.IdSuperior
                }; 

                Repository.Insert(entity);
                return Ok();
            }

            return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
        }


        [HttpPut("{id}")]
        public IActionResult Put(DepartamentoDTO dto)
        {
            DepartamentoValidator validator = new();

            var resultados = validator.Validate(dto);

            if (resultados.IsValid)
            {
                var entidadDept = Repository.Get(dto.Id ?? 0);

                if (entidadDept == null)
                {
                    return NotFound();
                }
                else
                {
                    entidadDept.Nombre = dto.NombreDepartamento;
                    entidadDept.Username = dto.Username;
                    entidadDept.Password = dto.Password;
                    entidadDept.IdSuperior = dto.IdSuperior;

                    Repository.Update(entidadDept);

                    return Ok();
                }

            }

            return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entidadDept = Repository.Get(id);

            if (entidadDept == null)
            {
                return NotFound();
            }

            Repository.Delete(entidadDept);

            return Ok();
        }
    }
}
