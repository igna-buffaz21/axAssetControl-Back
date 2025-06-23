using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.UsuarioDTO;
using axAssetControl.Negocio.Seguridad;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace axAssetControl.AccesoDatos
{
    public class AuthAD
    {
        private readonly AxAssetControlDbContext _context;
        private readonly JwtN _jwtN;

        public AuthAD(AxAssetControlDbContext context, JwtN jwtN)
        {
            _context = context;
            _jwtN = jwtN;
        }

        public async Task<string?> IniciarSesion(IniciarSesionDTO iniciarSesionDTO)
        {
            var user = await _context.Users.Where(u => u.Email == iniciarSesionDTO.Email)
                .Include(c => c.IdCompanyNavigation).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UnauthorizedAccessException("Credenciales incorrectas.");
            }

            if (user.IdCompanyNavigation.Status != "actived")
            {
                throw new InvalidOperationException("La empresa está dada de baja. Contacte al administrador.");
            }

            if (user.Status != "actived")
            {
                throw new InvalidOperationException("El usuario está dado de baja y no puede iniciar sesión.");
            }

            var hasher = new PasswordHasher<object>();
            var resultado = hasher.VerifyHashedPassword(null, user.Password, iniciarSesionDTO.Password);

            if (resultado != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedAccessException("Credenciales incorrectas.");
            }

            return _jwtN.GenerarToken(user);
        }

        public async Task<User> VerificarUsuarioExiste(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el usuario", ex);
            }
        }

        public async Task InsertarToken(UserPasswordReset userPasswordReset)
        {
            try
            {
                await _context.UserPasswordResets.AddAsync(userPasswordReset);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Insertar Token", ex);
            }
        }

        public async Task<UserPasswordReset> VerificarToken(string token)
        {
            try
            {
                var verificaciontoken = await _context.UserPasswordResets.FirstOrDefaultAsync(u => u.Token == token);

                if (verificaciontoken == null)
                {
                    return null;
                }

                return verificaciontoken;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Verificar Token", ex);
            }
        }

        public async Task<User> VerificarUsuarioPorID(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Verificar Usuario", ex);
            }
        }
    }
}
