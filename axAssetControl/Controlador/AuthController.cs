using System.IdentityModel.Tokens.Jwt;
using System.Text;
using axAssetControl.AccesoDatos;
using axAssetControl.Configuraciones;
using axAssetControl.Entidades.Dtos.UsuarioDTO;
using axAssetControl.Negocio;
using axAssetControl.Negocio.Seguridad;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System;

namespace axAssetControl.Controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthAD _authAD;
        private readonly JwtSettings _jwtSettings;
        private readonly AuthN _authN;

        public AuthController(AuthAD authAD, IOptions<JwtSettings> jwtSettings, AuthN authN)
        {
            _authAD = authAD;
            _jwtSettings = jwtSettings.Value;
            _authN = authN;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] IniciarSesionDTO iniciarSesionDTO )
        {

            try
            {
                var token = await _authAD.IniciarSesion(iniciarSesionDTO);

                if (token == null)
                {
                    return Unauthorized(new { mensaje = "Credenciales incorrectas!" });
                }
                return Ok(new { token });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(403, new { mensaje = ex.Message }); // ✅ Esto está perfecto
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensaje = ex.Message }); // mensaje más claro
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al iniciar sesion, intentelo mas tarde!" }); //cod 500
            }
        }

        [HttpPost("ValidarToken")]
        public IActionResult ValidarToken([FromBody] string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ClockSkew = TimeSpan.FromMinutes(5) // tolerancia por diferencia horaria
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                return Ok(new
                {
                    Valid = true,
                    Claims = principal.Claims.Select(c => new { c.Type, c.Value })
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Valid = false,
                    Error = ex.Message
                });
            }
        }

        [HttpPost("RecuperarPassword")]
        public async Task<IActionResult> RecuperarPass([FromBody] RecuperarPassDTO rpDTO )
        {
            try
            {
                var email = rpDTO.Email;
                await _authN.RecuperarContraseña(email);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut("CambiarPassword")]
        public async Task<IActionResult> CambiarPass([FromBody] CambiarPassDTO cpDTO)
        {
            try
            {
                var token = cpDTO.Token;
                var newPassword = cpDTO.newPassword;

                await _authN.CambiarContrasena(token, newPassword);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
