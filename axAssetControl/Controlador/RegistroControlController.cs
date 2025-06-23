using axAssetControl.Entidades.Dtos;
using axAssetControl.Entidades.Dtos.RegistroControlDTO;
using axAssetControl.Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace axAssetControl.Controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroControlController : ControllerBase
    {
        private readonly RegistroControlN _registroControlNegocio;

        public RegistroControlController(RegistroControlN registroControlNegocio)
        {
            _registroControlNegocio = registroControlNegocio;
        }

        [HttpGet("ObtenerHistorialPorSubSector")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> GetLog([FromQuery] int idSubSector)
        {
            var registroControl = await _registroControlNegocio.ObtenerRegistrosControl(idSubSector);
            if (registroControl == null) return NotFound(); ///cod 404
            return Ok(registroControl); //cod 200
        }

        /*[HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var registroControl = await _registroControlNegocio.ObtenerRegistrosControlPorId(id);
            if (registroControl == null) return NotFound(); //cod 404
            return Ok(registroControl); //cod 200

        }*/

        [HttpPost]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> Post([FromBody] CrearRegistroControlDTO registroControlDTO)
        {
            try
            {
                await _registroControlNegocio.CrearRegistroControl(registroControlDTO);
                return Ok("registro Control creado exitosamente");///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al crear el registro Control " + ex.Message); //cod 500
            }
        }

        /*[HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _registroControlNegocio.EliminarRegistroControl(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); //error de validación
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); //error inesperado
            }
        }*/

        [HttpGet("ObtenerUltimoRegistroControl")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> GetRegistroControl(int idSubsector)
        {
            try
            {
                var registroContro = await _registroControlNegocio.ObtenerUltimoControl(idSubsector);
                return Ok(registroContro);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al crear el registro Control " + ex.Message); //cod 500
            }
        }

        /*[HttpGet("ObtenerUltimoControlPorNombreActivo")]
                [Authorize]
        public async Task<IActionResult> GetRegistroControlForName([FromQuery] int idSubsector, string nombre)
        {
            try
            {
                var registroContro = await _registroControlNegocio.ObtenerUltimoControlPorNombreActivo(idSubsector, nombre);
                return Ok(registroContro);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al crear el registro Control " + ex.Message); //cod 500
            }
        } */

        [HttpGet("ObtenerRegistroControlEspecifico")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> GetRegistroEspecifico(int id, int idCompany)
        {
            try
            {
                var registroControl = await _registroControlNegocio.ObtenerControlEspecifico(id, idCompany);
                return Ok(registroControl);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al obtener el registro Control " + ex.Message); //cod 500
            }
        }

        [HttpGet("ObtenerActivosPerdidos")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> GetActivosPerdidos([FromQuery] int idCompany)
        {
            try
            {
                var activosPerdidos = await _registroControlNegocio.ObtenerActivosPerdidos(idCompany);
                return Ok(activosPerdidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al obtener el registro Control " + ex.Message); //cod 500
            }
        }

        ///ver si se puede actualizar un registroControl
        /*
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] ActualizarEmpresaDTO empresaDTO)
        {
            try
            {
                await _empresaNegocio.ActualizarEmpresa(empresaDTO);
                return Ok("Empresa editada exitosamente");///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al editar la empresa " + ex.Message); //cod 500
            }
        } */
    }
}
