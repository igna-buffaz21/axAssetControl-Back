using axAssetControl.Entidades.Dtos.LocacionDTO;
using axAssetControl.Entidades.Dtos.SectorDTO;
using axAssetControl.Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace axAssetControl.Controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectorController : ControllerBase
    {
        private readonly SectorN _sectorNegocio;

        public SectorController(SectorN sectorNegocio)
        {
            _sectorNegocio = sectorNegocio;
        }

        [HttpGet("Locacion/{idcompany}")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> Get(int idcompany, [FromQuery] int idEmpresa, bool status)
        {
            Console.WriteLine(status);

            var sector = await _sectorNegocio.ObtenerSectores(idcompany, idEmpresa, status);
            if (sector == null) return NotFound(); ///cod 404
            return Ok(sector); //cod 200
        }

        /* [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetForId(int id)
        {
            var sector = await _sectorNegocio.ObtenerSectorPorId(id);
            if (sector == null) return NotFound(); //cod 404
            return Ok(sector); //cod 200

        } */

        [HttpPost("CrearSector")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromBody] CrearSectorDTO sectorDTO)
        {
            try
            {
                await _sectorNegocio.CrearSector(sectorDTO);
                return Ok(new {mensaje = "Sector creado con exito"});///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {mensaje = ex.Message}); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {mensaje = "Error interno al crear el sector, intentelo mas tarde!" }); //cod 500
            }
        }

        /*[HttpDelete("EliminarSector/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sectorNegocio.EliminarSector(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {mensaje = ex.Message}); //error de validación
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {mensaje = "Error interno al borrar el sector, intentelo mas tarde!" }); //error inesperado
            }
        }*/

        [HttpPut("CambiarEstado")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateStatus([FromBody] int id)
        {
            try
            {
                await _sectorNegocio.CambiarEstado(id);
                return Ok(new { mensaje = "sector editado con exito" });///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message }); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar el sector, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpPut("ActualizarSector")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromBody] ActualizarSectorDTO sectorDTO)
        {
            try
            {
                await _sectorNegocio.ActualizarSector(sectorDTO);
                return Ok(new {mensaje = "Sector actualizado con exito"});///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al actualizar el sector, intentelo mas tarde!"); //cod 500
            }
        }

        [HttpGet("ObtenerSectorPorNombre/{idLocacion}")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> GetForName(int idLocacion, [FromQuery] string name)
        {
            try
            {
                var sectores = await _sectorNegocio.ObtenerSectorPorNombre(idLocacion, name);
                return Ok(sectores);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message }); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al obtener los sectores, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpGet("FiltarSectores/{idLocacion}")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> GetForFilt(int idLocacion, [FromQuery] string orden)
        {
            try
            {
                var sectores = await _sectorNegocio.FiltarSectores(idLocacion, orden);
                return Ok(sectores);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message }); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al obtener los sectores, intentelo mas tarde!" }); //cod 500
            }
        }
    }
}
