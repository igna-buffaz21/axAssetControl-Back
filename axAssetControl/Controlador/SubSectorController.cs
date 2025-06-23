using axAssetControl.Entidades.Dtos;
using axAssetControl.Entidades.Dtos.SubSectorDTO;
using axAssetControl.Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace axAssetControl.Controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubSectorController : ControllerBase
    {
        private readonly SubSectorN _subSectorNegocio;

        public SubSectorController(SubSectorN subSectorNegocio)
        {
            _subSectorNegocio = subSectorNegocio;
        }

        [HttpGet("Sector/{idsector}")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> Get(int idsector, [FromQuery] int idEmpresa)
        {
            var subSectores = await _subSectorNegocio.ObtenerSubSectores(idsector, idEmpresa);
            if (subSectores == null) return NotFound(); ///cod 404
            return Ok(subSectores); //cod 200
        }

        /*[HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetForId(int id)
        {
            var subSector = await _subSectorNegocio.ObtenerSubSectorPorId(id);
            if (subSector == null) return NotFound(); //cod 404
            return Ok(subSector); //cod 200

        } */

        [HttpPost("CrearSubSector")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromBody] CrearSubSectorDTO subSectorDTO)
        {
            try
            {
                await _subSectorNegocio.CrearSubSector(subSectorDTO);
                return Ok(new {mensaje = "Subsector creado con exito"});///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {mensaje =  ex.Message}); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {mensaje = "Error interno al crear el subsector, intentelo mas tarde! /" + ex.Message + ex.InnerException  }); //cod 500
            }
        }

        [HttpDelete("EliminarSubSector/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _subSectorNegocio.EliminarSubSector(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {mensaje = ex.Message}); //error de validación
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {mensaje = "Error interno al borrar el subsector, intentelo mas tarde!"}); //error inesperado
            }
        }

        [HttpPut("ActualizarSubSector")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromBody] ActualizarSubSectorDTO subSectorDTO)
        {
            try
            {
                await _subSectorNegocio.ActualizarSubSector(subSectorDTO);
                return Ok(new {mensaje = "Subsector editado con exito"});///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {mensaje = ex.Message}); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {mensaje = "Error interno al actualizar el subsector, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpGet("ObtenerSubSectorPorNombre/{idSector}")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> GetForName(int idSector, [FromQuery] string name)
        {
            try
            {
                var subSectores = await _subSectorNegocio.ObtenerSubSectorPorNombre(idSector, name);
                return Ok(subSectores);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message }); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al obtener los subsectores, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpGet("FiltarSubSectores/{idSector}")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> GetForFilt(int idSector, [FromQuery] string orden)
        {
            try
            {
                var subSectores = await _subSectorNegocio.FiltarSubSectores(idSector, orden);
                return Ok(subSectores);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message }); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al obtener los subsectores, intentelo mas tarde!" }); //cod 500
            }
        }
    }
}
