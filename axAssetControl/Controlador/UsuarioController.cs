using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.UsuarioDTO;
using axAssetControl.Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace axAssetControl.Controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioN _usuarioNegocio;

        public UsuarioController(UsuarioN usuarioNegocio)
        {
            _usuarioNegocio = usuarioNegocio;
        }

        [HttpGet("ObtenerUsuariosPorCompania/{idCompany}")]
        [Authorize(Roles = "admin, operator")]
        public async Task<IActionResult> Get(int idCompany, [FromQuery] string status)
        {

            if (status != "actived" && status != "disabled")
            {
                return BadRequest("El 'status' debe ser 'active' o 'disabled'.");
            }

            Console.WriteLine(status);

            var usuarios = await _usuarioNegocio.ObtenerUsuarios(idCompany, status);
            return Ok(usuarios); //cod 200
        }

        [HttpGet("obtenerDatosUsuario/{id}")]
        [Authorize(Roles = "admin, operator, superadmin")]
        public async Task<IActionResult> GetForId(int id)
        {
            try
            {
                var usuario = await _usuarioNegocio.obtenerDatosUsuario(id);

                return Ok(usuario); //cod 200
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al obtener el usuario, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpPost("CrearUsuario")]
        [Authorize(Roles = "admin, superadmin")]
        public async Task<IActionResult> Post([FromBody] CrearUsuarioDTO usuarioDTO)
        {
            try
            {
                await _usuarioNegocio.CrearUsuario(usuarioDTO);
                return Ok(new {mensaje = "Usuario creado con exito"});///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {mensaje = ex.Message}); //cod 400
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { mensaje = "Error interno al crear el usuario, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpPost("CrearUsuarioEnCantidad")]
        [Authorize(Roles = "admin, superadmin")]
        public async Task<IActionResult> PostUsers([FromBody] List<CrearUsuarioDTO> usersDTO)
        {
            try
            {
                await _usuarioNegocio.CrearUsuariosEnCantidad(usersDTO);
                return Ok(new { mensaje = "Usuarios creados con exito" });///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message }); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al crear los usuarios, intentelo mas tarde!" }); //cod 500
            }
        }

        /*[HttpDelete("EliminarUsuario/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _usuarioNegocio.EliminarUsuario(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message }); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al eliminar el usuario, intentelo mas tarde!" }); //cod 500
            }
        } */

        [HttpPut("ActualizarUsuario")]
        [Authorize(Roles = "admin, superadmin")]
        public async Task<IActionResult> Update([FromBody] ActualizarNombreUsuarioDTO usuarioDTO)
        {
            try
            {
                await _usuarioNegocio.ActualizarUsuario(usuarioDTO);
                return Ok(new { mensaje = "Usuario actualizado con exito" });///cod 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); //cod 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar el usuario, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpPut("BajaUsuario")]
        [Authorize(Roles = "admin, superadmin")]
        public async Task<IActionResult> UpdateStatusB([FromBody] int id)
        {
            try
            {
                await _usuarioNegocio.BajaUsuario(id);
                return Ok(new { mensaje = "Usuario dado de baja con exito" });///cod 200
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar el usuario, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpPut("AltaUsuario")]
        [Authorize(Roles = "admin, superadmin")]
        public async Task<IActionResult> UpdateStatusA([FromBody] int id)
        {
            try
            {
                await _usuarioNegocio.AltaUsuario(id);
                return Ok(new { mensaje = "Usuario dado de alta con exito" });///cod 200
            }

            catch (ArgumentException ex)
            {
                return BadRequest(new {mensaje = "Error en los parametros"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar el usuario, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpGet("ObtenerUsuarioPorNombre")]
        [Authorize(Roles = "admin, superadmin, operator")]
        public async Task<IActionResult> GetUserForName([FromQuery] string name, string status, int idCompany)
        {
            try
            {
                var usuarios = await _usuarioNegocio.ObtenerUsuarioPorNombre(name, status, idCompany);
                return Ok(usuarios); //cod 200
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al obtener el usuario, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpGet("ObtenerAdministradores")]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> GetAdmins([FromQuery] string role, bool status)
        {
            try
            {
                var usuarios = await _usuarioNegocio.ObtenerAdministradores(role, status);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al obtener el usuario, intentelo mas tarde!" }); //cod 500
            }
        }

        /*[HttpGet("CheckRole")]
        [Authorize(Roles = "admin")]
        public IActionResult CheckRole()
        {
            var claims = User.Claims.Select(c => $"{c.Type}: {c.Value}");
            return Ok(claims);
        } */
    }
}
