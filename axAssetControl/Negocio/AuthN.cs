using System.Security.Cryptography;
using System.Web;
using axAssetControl.AccesoDatos;
using axAssetControl.Entidades;
using axAssetControl.Mapeo;
using Microsoft.AspNetCore.Identity;

namespace axAssetControl.Negocio
{
    public class AuthN
    {
        private readonly AuthAD _authAD;
        private readonly SendMail _sendMail;
        private readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();
        private readonly AxAssetControlDbContext _context;


        public AuthN(AuthAD authAD, SendMail sendMail, AxAssetControlDbContext context)
        {
            _context = context;
            _authAD = authAD;
            _sendMail = sendMail;
        }

        public async Task RecuperarContraseña(string email)
        {

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException("Email no valido");
            }

            var user = await _authAD.VerificarUsuarioExiste(email);

            if (user == null)
            {
                throw new ArgumentException("No hay ningun usuario vinculado a este correo");
            }

            var token = GenerateSecureToken();

            var expirationUnix = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds();

            var used = false;

            var passwordReset = new UserPasswordReset
            {
                IdUser = user.Id,
                Token = token,
                Expiracion = expirationUnix,
                Used = used
            };

            await _authAD.InsertarToken(passwordReset);

            var encodedToken = HttpUtility.UrlEncode(token);

            var resetLink = "http://localhost:4200/auth/reset-password?token=" + encodedToken;

            var body = $@"
                <!DOCTYPE html>
                <html lang=""es"">
                <head>
                  <meta charset=""UTF-8"" />
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
                  <title>Recuperar contraseña</title>
                </head>
                <body style=""margin:0; padding:0; background:#f5f7fa; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; color:#333;"">
                  <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""background:#f5f7fa; padding: 40px 0;"">
                    <tr>
                      <td align=""center"">
                        <table role=""presentation"" width=""400"" cellspacing=""0"" cellpadding=""0"" style=""background:#ffffff; border-radius:8px; box-shadow:0 2px 8px rgba(0,0,0,0.1); padding: 30px;"">
                          <tr>
                            <td style=""text-align:center; padding-bottom: 20px;"">
                              <h1 style=""margin:0; font-weight:700; font-size:24px; color:#2d3748;"">Recuperación de Contraseña</h1>
                            </td>
                          </tr>
                          <tr>
                            <td style=""font-size:16px; line-height:1.5; color:#4a5568; padding-bottom: 20px;"">
                              Hola,<br />
                              Recibimos una solicitud para restablecer la contraseña de tu cuenta. Haz clic en el botón de abajo para continuar.
                            </td>
                          </tr>
                          <tr>
                            <td align=""center"" style=""padding-bottom: 30px;"">
                              <a href=""{resetLink}"" target=""_blank"" style=""background:#3182ce; color:#fff; text-decoration:none; padding:12px 25px; border-radius:6px; font-weight:600; display:inline-block;"">
                                Restablecer Contraseña
                              </a>
                            </td>
                          </tr>
                          <tr>
                            <td style=""font-size:14px; line-height:1.4; color:#a0aec0; border-top:1px solid #e2e8f0; padding-top:20px;"">
                              Si no solicitaste este cambio, puedes ignorar este mensaje.<br />
                              Este enlace expirará en 15 minutos.
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


                await _sendMail.SendEmail(user.Email, "Recuperar Contraseña", body);

        }

        public async Task CambiarContrasena(string token, string newpassword)
        {
            try
            {
                var verificacionToken = await _authAD.VerificarToken(token);

                long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                if (verificacionToken == null)
                {
                    throw new ArgumentException("Token invalido");
                }

                if (verificacionToken.Used)
                {
                    throw new ArgumentException("Token usado");
                }

                if (verificacionToken.Expiracion < unixTimestamp)
                {
                    throw new ArgumentException("Token expirado");
                }

                var user = await _authAD.VerificarUsuarioPorID(verificacionToken.IdUser);

                if (user == null)
                {
                    throw new ArgumentException("Usuario no encontrado");
                }

                user.Password = _hasher.HashPassword(user, newpassword);

                verificacionToken.Used = true;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex.Message);
            }

        }

        public static string GenerateSecureToken(int length = 32)
        {
            var randomBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
    }
}
