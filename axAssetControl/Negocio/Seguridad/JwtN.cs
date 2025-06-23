using System.Security.Claims;
using axAssetControl.Configuraciones;
using axAssetControl.Entidades;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace axAssetControl.Negocio.Seguridad
{
    public class JwtN
    {
        private readonly JwtSettings _jwtSettings;

        public JwtN(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerarToken(User usuario)
        {
            var claims = new[] ///datos del usuario para generar un token unico
            {
                new Claim("companyId", usuario.IdCompany.ToString()), ///id de la compania
                new Claim("userId", usuario.Id.ToString()), ///id del usuario
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), ///id del jwt, asegurandose que sea unico
                new Claim(ClaimTypes.Role, usuario.Rol) 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)); ///convierte la clave del appsetting.json en un arreglo de bytes
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); ///esto nos permite firmar el token con la key de arriba

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer, ///quien emite el token
                audience: _jwtSettings.Audience, ///quien lo recibe
                claims: claims, ///datos del usuario
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes), ///duracion del token
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
