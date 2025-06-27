using axAssetControl.Entidades.Dtos.LocacionDTO;
using axAssetControl.Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace axAssetControl.Controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocacionController : ControllerBase
    {
        private readonly LocacionN _locacionNegocio;

        public LocacionController(LocacionN locacionNegocio)
        {
            _locacionNegocio = locacionNegocio;
        }

        [HttpGet("Empresa/{idcompany}")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> Get(int idcompany, [FromQuery] bool status)
        {
            var locaciones = await _locacionNegocio.ObtenerLocaciones(idcompany, status);
            if (locaciones == null) return NotFound(); ///cod 404
            return Ok(locaciones); //cod 200
        }

        /*[HttpGet("{id}")]
        public async Task<IActionResult> GetForId(int id)
        {
            var locaciones = await _locacionNegocio.ObtenerLocacionPorId(id);
            if (locaciones == null) return NotFound(); //cod 404
            return Ok(locaciones); //cod 200

        }*/

        [HttpPost("CrearLocacion")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromBody] CrearLocacionDTO locacionDTO)
        {
            try
            {
                await _locacionNegocio.CrearLocacion(locacionDTO);
                return Ok(new {mensaje = "Locacion creada con exito"});///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {mensaje = "Error interno al crear la locacion, intentelo mas tarde!" }); //cod 500
            }
        }

        /*[HttpDelete("EliminarLocacion/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _locacionNegocio.EliminarLocacion(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {mensaje = ex.Message}); //error de validación
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {mensaje = "Error interno al borrar la locacion, intentelo mas tarde!" }); //error inesperado
            }
        }*/

        [HttpPut("ActualizarLocacion")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromBody] ActualizarLocacionDTO locacionDTO)
        {
            try
            {
                await _locacionNegocio.ActualizarLocacion(locacionDTO);
                return Ok(new {mensaje = "Locacion editada con exito"});///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {mensaje = ex.Message}); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {mensaje = "Error interno al actualizar la locacion, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpPut("CambiarEstado")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateStatus([FromBody] CambiarEstadoDTO locacionDTO)
        {
            try
            {
                await _locacionNegocio.CambiarEstado(locacionDTO);
                return Ok(new { mensaje = "Locacion editada con exito" });///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message }); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar la locacion, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpGet("ObtenerLocacionPorNombre/{idCompany}")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> GetForName(int idCompany, [FromQuery] string name)
        {
            try
            {
                var locaciones = await _locacionNegocio.ObtenerLocacionesPorNombre(idCompany, name);
                return Ok(locaciones);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message }); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar la locacion, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpGet("FiltarLocaciones/{idCompany}")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> GetForFilt(int idCompany, [FromQuery] string orden)
        {
            try
            {
                var locaciones = await _locacionNegocio.FiltarLocaciones(idCompany, orden);
                return Ok(locaciones);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message }); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar la locacion, intentelo mas tarde!" }); //cod 500
            }
        }
    }
}
