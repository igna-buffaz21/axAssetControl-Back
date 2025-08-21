using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.EmpresaDTO;
using axAssetControl.Mapeo;
using axAssetControl.Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace axAssetControl.Controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly EmpresaN _empresaNegocio;

        public EmpresaController(EmpresaN empresaNegocio)
        {
            _empresaNegocio = empresaNegocio;
        }

        [HttpGet("ObtenerEmpresas")]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Get([FromQuery] bool mostrarInactivos)
        {
            try
            {
                var empresas = await _empresaNegocio.ObtenerEmpresas(mostrarInactivos);
                return Ok(empresas); //cod 200
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener las empresas, intentelo de nuevo mas tarde!" });
            }
        }

        /*[HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var empresa = await _empresaNegocio.ObtenerEmpresaPorId(id);
            if (empresa == null) return NotFound(); //cod 404
            return Ok(empresa); //cod 200

        }*/

        [HttpPost("CrearEmpresa")]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Post([FromBody] CrearEmpresaDTO empresaDTO)
        {
            try
            {
                await _empresaNegocio.CrearEmpresa(empresaDTO);
                return Ok(new { mensaje = "Empresa creada con exito" });///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al crear la empresa " + ex.Message); //cod 500
            }
        }

        /*[HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _empresaNegocio.EliminarEmpresa(id);
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

        [HttpPut("ActualizarEmpresa")]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Update([FromBody] ActualizarEmpresaDTO empresaDTO)
        {
            try
            {
                await _empresaNegocio.ActualizarEmpresa(empresaDTO);
                return Ok(new {mensaje = "Empresa actualizada con exito"});///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al editar la empresa " + ex.Message); //cod 500
            }
        }

        [HttpPut("AltaBajaEmpresa")]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> UpdateStatus([FromBody] ActualizarEstadoEmpresaDTO empresaDTO)
        {
            try
            {
                await _empresaNegocio.AltaBajaEmpresa(empresaDTO);
                return Ok(new { mensaje = "Estado actualizado con exito!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {mensaje = ex.Message});
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al editar la empresa " + ex.Message); //cod 500
            }
        }

        [HttpGet("ObtenerNombredeEmpresPorId")]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            try
            {
               var empresa = await _empresaNegocio.ObtenerNombreEmpresaPorId(id);

                return Ok(new { mensaje = empresa });

            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener el nombre de la empresa" + ex.Message); //cod 500
            }
        }
    }
}
