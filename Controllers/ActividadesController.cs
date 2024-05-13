using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_U3.Models.Validators;
using U3Api.Models.DTOs;
using U3Api.Models.Entities;
using U3Api.Repositories;

namespace Proyecto_U3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadesController : ControllerBase
    {
        public Repository<Actividades> Repo { get; }

        public ActividadesController(Repository<Actividades> repo)
        {
            Repo = repo;
        }

        [HttpGet]
        public IActionResult GetAllActividades()
        {
            //Parametros de fecha.

            var actividades = Repo.GetAll().
                OrderBy(x => x.FechaCreacion).
                Select(x => new ActividadDTO
                {
                    Id = x.Id,
                    Titulo = x.Titulo,
                    Descripcion = x.Descripcion,
                    IdDepartamento = x.IdDepartamento,
                    FechaDeCreacion = x.FechaCreacion,
                    FechaDeRealizacion = x.FechaRealizacion,
                    Estado = x.Estado
                });
            return Ok(actividades);
        }

        [HttpPost]
        public IActionResult PostAct(ActividadDTO dto)
        {
            ActividadValidator validator = new();
            var resultados = validator.Validate(dto);

            if (resultados.IsValid)
            {
                Actividades entidad = new()
                {
                    Id = 0,
                    Titulo = dto.Titulo,
                    Descripcion = dto.Descripcion,
                    FechaRealizacion = dto.FechaDeRealizacion,
                    IdDepartamento = dto.IdDepartamento,
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow,
                    Estado = dto.Estado
                };
                Repo.Insert(entidad);
                return Ok();
        }

            return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
        }


        [HttpPut("{id}")]
        public IActionResult PutAct(ActividadDTO dto)
        {
            ActividadValidator validator = new();
            var resultados = validator.Validate(dto);

            if (resultados.IsValid)
            {
                var actividad = Repo.Get(dto.Id ?? 0);

                if (actividad == null || actividad.Estado == 3)
                {
                    return NotFound();
                }
                else
                {
                    actividad.Titulo = dto.Titulo;
                    actividad.Descripcion = dto.Descripcion;
                    actividad.FechaRealizacion = dto.FechaDeRealizacion;
                    actividad.IdDepartamento = dto.IdDepartamento;
                    actividad.FechaCreacion = dto.FechaDeCreacion;
                    actividad.FechaActualizacion = DateTime.UtcNow;
                    actividad.Estado = dto.Estado;  

                    Repo.Update(actividad);

                    return Ok();
                }
            }

            return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var actividad = Repo.Get(id);

            if(actividad == null || actividad.Estado == 3)
            {
                return NotFound();
            }

            actividad.Estado = 3;
            actividad.FechaActualizacion = DateTime.UtcNow;
            Repo.Update(actividad);
            return Ok();
        }
    }
}
