using axAssetControl.Entidades.Dtos;
using axAssetControl.Entidades.Dtos.ActivoDTO;
using axAssetControl.Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace axAssetControl.Controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivoController : ControllerBase
    {
        private readonly ActivoN _activoNegocio;

        public ActivoController(ActivoN activoNegocio)
        {
            _activoNegocio = activoNegocio;
        }

        [HttpGet("SubSector/{idsubsector}")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> Get(int idsubsector, [FromQuery] int idEmpresa)
        {
            var activos = await _activoNegocio.ObtenerActivos(idsubsector, idEmpresa);
            if (activos == null) return NotFound(); ///cod 404 ///nunca null ///ver
            return Ok(activos); //cod 200
        }

        /*[HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var activo = await _activoNegocio.ObtenerActivoPorId(id);
            if (activo == null) return NotFound(); //cod 404 ///nunca null ///ver
            return Ok(activo); //cod 200
        }*/

        [HttpPost("CrearActivo")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromBody] CrearActivoDTO activoDTO)
        {
            try
            {
                await _activoNegocio.CrearActivo(activoDTO);
                return Ok(new { mensaje = "Activo creado con exito" });///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {mensaje = ex.Message}); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {mensaje = "Error interno al crear el activo, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpDelete("EliminarActivo/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _activoNegocio.EliminarActivo(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {mensaje = ex.Message}); //error de validación
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {mensaje = "Error interno al borrar el activo, intentelo mas tarde!" }); //error inesperado
            }
        }

        [HttpPut("ActualizarActivo")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromBody] ActualizarActivoDTO activoDTO)
        {
            try
            {
                await _activoNegocio.ActualizarActivo(activoDTO);
                return Ok(new {mensaje = "Activo actualizado con exito"});///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {mensaje = ex.Message}); //cod 400
            }
            catch (Exception ex)
            {
                //return StatusCode(500, new {mensaje = "Error interno al actualizar el activo, intentelo mas tarde!" }); //cod 500
                return StatusCode(500, ex.Message + " / " + ex.InnerException); //cod 500
            }
        }

        [HttpGet("ObtenerActivoPorNombre/{idSubSector}")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> GetForName(int idSubSector, [FromQuery] string name)
        {
            try
            {
                var activos = await _activoNegocio.ObtenerActivoPorNombre(idSubSector, name);
                return Ok(activos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message }); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al obtener los activos, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpGet("FiltarActivos/{idSubSector}")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> GetForFilt(int idSubSector, [FromQuery] string orden)
        {
            try
            {
                var activos = await _activoNegocio.FiltarActivos(idSubSector, orden);
                return Ok(activos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message }); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al obtener los activos, intentelo mas tarde!" }); //cod 500
            }
        }
    }
}
