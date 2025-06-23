using axAssetControl.Entidades.Dtos;
using axAssetControl.Entidades.Dtos.DetalleControlDTO;
using axAssetControl.Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace axAssetControl.Controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleControlController : ControllerBase
    {
        private readonly DetalleControlN _detalleControlNegocio;

        public DetalleControlController(DetalleControlN detalleControlNegocio)
        {
            _detalleControlNegocio = detalleControlNegocio;
        }

        [HttpGet]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> Get()
        {
            var detalleControl = await _detalleControlNegocio.ObtenerDetalleControl();
            if (detalleControl == null) return NotFound(); ///cod 404
            return Ok(detalleControl); //cod 200
        }

        /*[HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var detalleControl = await _detalleControlNegocio.ObtenerDetalleControlPorId(id);
            if (detalleControl == null) return NotFound(); //cod 404
            return Ok(detalleControl); //cod 200

        }*/

        [HttpPost]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> Post([FromBody] CrearDetalleControlDTO detalleControlDTO)
        {
            try
            {
                await _detalleControlNegocio.CrearDetalleControl(detalleControlDTO);
                return Ok("detalle control creado exitosamente");///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al crear el detalle control " + ex.Message); //cod 500
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _detalleControlNegocio.EliminarDetalleControl(id);
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
        }

        [HttpPut]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> Update([FromBody] ActualizarDetalleControlDTO detalleControlDTO)
        {
            try
            {
                await _detalleControlNegocio.ActualizarDetalleControl(detalleControlDTO);
                return Ok("Detalle control editado exitosamente");///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al editar el detalle control " + ex.Message + " / " + ex.InnerException); //cod 500
            }
        }
    }
}
