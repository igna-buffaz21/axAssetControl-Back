using axAssetControl.AccesoDatos;
using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.UsuarioDTO;
using axAssetControl.Mapeo;
using Microsoft.AspNetCore.Identity;

namespace axAssetControl.Negocio
{
    public class UsuarioN 
    {
        private readonly UsuarioAD _usuarioAD;
        private readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();
        private readonly SendMail _sendMail;

        public UsuarioN(UsuarioAD usuarioAD, SendMail sendMail) 
        {
            _sendMail = sendMail;
            _usuarioAD = usuarioAD;
        }

        public async Task<List<ObtenerUsuarioDTO>> ObtenerUsuarios(int idCompany, string status)
        {
            var usuario = await _usuarioAD.ObtenerTodos(idCompany, status);

            var usuarioDTO = MapeoUsuario.ObtenerUsuariosMAP(usuario);

            return usuarioDTO;
        }

        public async Task<User> ObtenerUsuarioPorId(int id)
        {
            return await _usuarioAD.ObtenerPorId(id);
        }
        
        public async Task CrearUsuario(CrearUsuarioDTO usuarioDTO)
        {

            var usuario = MapeoUsuario.CrearUsuarioMAP(usuarioDTO);

            if (string.IsNullOrWhiteSpace(usuario.Name))
            {
                throw new ArgumentException("El nombre del usuario es obligatorio.");
            }///Validacion nombre usuario

            if(string.IsNullOrWhiteSpace(usuario.Email))
            {
                throw new ArgumentException("El email del usuario es obligatorio.");
            }///Validacion email usuario
            else if (!IsValidEmail(usuario.Email))
            {
                throw new ArgumentException("El email del usuario no es válido.");
            }///Validacion email

            if (string.IsNullOrWhiteSpace(usuario.Password))
            {
                throw new ArgumentException("La contraseña del usuario es obligatoria.");
            }///Validacion contraseñá usuario

            if (usuario.Rol == null)
            {
                throw new ArgumentException("El rol del usuario es obligatorio.");
            }///Validacion Rol

            if (string.IsNullOrWhiteSpace(usuario.Status))
            {
                throw new ArgumentException("El estado del usuario es obligatorio.");
            }

            usuario.Status = usuario.Status.Trim();

            usuario.Password = _hasher.HashPassword(null, usuario.Password);

            await _usuarioAD.Agregar(usuario);

            var loginLink = "http://localhost:4200/auth/login";

            var body = $@"
                <!DOCTYPE html>
                <html lang=""es"">
                <head>
                  <meta charset=""UTF-8"" />
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1"" 
                  <title>Bienvenido {usuario.Name}</title>
                </head>
                <body style=""margin:0; padding:0; background:#f5f7fa; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; color:#333;"">
                  <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""background:#f5f7fa; padding: 40px 0;"">
                    <tr>
                      <td align=""center"">
                        <table role=""presentation"" width=""400"" cellspacing=""0"" cellpadding=""0"" style=""background:#ffffff; border-radius:8px; box-shadow:0 2px 8px rgba(0,0,0,0.1); padding: 30px;"">
                          <tr>
                            <td style=""text-align:center; padding-bottom: 20px;"">
                              <h1 style=""margin:0; font-weight:700; font-size:24px; color:#2d3748;"">¡Bienvenido a axAssetControl!</h1>
                            </td>
                          </tr>
                          <tr>
                            <td style=""font-size:16px; line-height:1.5; color:#4a5568; padding-bottom: 20px;"">
                              ¡Hola!<br /><br />
                              Gracias por registrarte en <strong>axAssetControl</strong>, tu solución para la gestión inteligente de activos.<br /><br />
                              Antes de comenzar, por seguridad debes <strong>restablecer tu contraseña</strong> para acceder a la plataforma.<br /><br />
                              Solo sigue estos pasos:
                              <ul style=""padding-left: 20px; margin: 10px 0; color: #4a5568;"">
                                <li>Dirígete a la página de inicio de sesión.</li>
                                <li>Haz clic en <strong>¿Olvidaste tu contraseña?</strong>.</li>
                                <li>Ingresa tu correo electrónico.</li>
                                <li>Recibirás un email con instrucciones para restablecer tu contraseña.</li>
                              </ul>
                              Una vez restablecida, ya podrás ingresar normalmente.
                            </td>
                          </tr>
                          <tr>
                            <td align=""center"" style=""padding-bottom: 30px;"">
                              <a href=""{loginLink}"" target=""_blank"" style=""background:#3182ce; color:#fff; text-decoration:none; padding:12px 25px; border-radius:6px; font-weight:600; display:inline-block;"">
                                Ir a la Plataforma
                              </a>
                            </td>
                          </tr>
                          <tr>
                            <td style=""font-size:14px; line-height:1.4; color:#a0aec0; border-top:1px solid #e2e8f0; padding-top:20px;"">
                              Si tienes preguntas o necesitas ayuda, no dudes en contactarnos.<br />
                              ¡Estamos aquí para ayudarte!
                            </td>
                          </tr>
                          <tr>
                            <td style=""font-size:12px; color:#cbd5e0; padding-top: 10px; text-align:center;"">
                              © 2025 Aumax. Todos los derechos reservados.
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </body>
                </html>
                ";

            await _sendMail.SendEmail(usuario.Email, "Bienvenido a axAssetControl", body);
        }

        public async Task CrearUsuariosEnCantidad(List<CrearUsuarioDTO> usersDTO)
        {

            Console.WriteLine($"USUARIOS LLEGADOS DE KOTLIN - Cantidad: {usersDTO.Count}");

            foreach (var usuario in usersDTO)
            {
                Console.WriteLine($"Usuario: {usuario.Name}, Email: {usuario.Email}, Company: {usuario.IdCompany}");
            }

            if (usersDTO.Count < 1)
            {
                throw new ArgumentException("Lista de usuarios vacia!");
            }

            var users = usersDTO.Select(u => new User
            {
                IdCompany = u.IdCompany,
                Name = u.Name,
                Email = u.Email,
                Password = _hasher.HashPassword(null, u.Password), // Hashear aquí
                Rol = u.Rol,
                Status = u.Status.Trim(), // Trim aquí
            }).ToList();

            await _usuarioAD.AgregarUsuariosEnCantidad(users);
        }
        
        public async Task EliminarUsuario(int id)
        {
            if(id == 0)
            {
                throw new ArgumentException("El id del usuario es obligatorio.");
            }
            await _usuarioAD.Eliminar(id);
        }
        
        public async Task ActualizarUsuario(ActualizarNombreUsuarioDTO usuarioDTO)
        {

            var usuario = MapeoUsuario.ActualizarNombreUsuarioMAP(usuarioDTO);

            if (string.IsNullOrWhiteSpace(usuario.Name))
            {
                throw new ArgumentException("El nombre del usuario es obligatorio.");
            }///Validacion nombre usuario

            await _usuarioAD.Actualizar(usuario);

        }
        
        public async Task BajaUsuario(int id)
        {
            await _usuarioAD.BajaUsuario(id);
        }

        public async Task AltaUsuario(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Id Nulo");
            }
            await _usuarioAD.AltaUsuario(id);
        }

        public async Task<List<ObtenerUsuarioDTO>> ObtenerUsuarioPorNombre(string name, string status, int idCompany)
        {

            var usuario = await _usuarioAD.ObtenerUsuarioPorNombre(name, status, idCompany);

            if (usuario == null)
            {
                throw new Exception("No se encontro ningun usuario llamado " +  name);
            }

            var usuarioDTO = MapeoUsuario.ObtenerUsuariosMAP(usuario);

            return usuarioDTO;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        } ///Metodo que verifica si el formato del email es valido

        public async Task<List<ObtenerAdministradorDTO>> ObtenerAdministradores(string role, bool status)
        {

            string s = "";

            if (status)
            {
                s = "disabled";
            }
            else if (!status)
            {
                s = "actived";
            }
            else
            {
                throw new Exception("Tipo rol invalido");
            }


            var usuarios = await _usuarioAD.ObtenerAdministradoresEmpresa(role, s);

            if (usuarios == null)
            {
                throw new Exception("No se encontro ningun usuario");
            }

            var usuariosDTO = MapeoUsuario.ObtenerAdministradorMAP(usuarios);

            return usuariosDTO;
        }

    }
}
