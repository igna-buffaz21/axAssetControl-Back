using System.Text;
using axAssetControl.Entidades;
using axAssetControl.Negocio;
using Microsoft.EntityFrameworkCore;

namespace axAssetControl.AccesoDatos
{
    public class DetalleControlAD
    {
        private readonly AxAssetControlDbContext _context;
        private readonly SendMail _sendMail;

        public DetalleControlAD(AxAssetControlDbContext context, SendMail sendMail)
        {
            _context = context;
            _sendMail = sendMail;
        }

        public async Task<List<DetailControl>> ObtenerTodos()
        {
            try
            {
                return await _context.DetailControls.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los detalles de control " + ex.Message);
            }
        }

        public async Task<DetailControl> ObtenerPorId(int id)
        {
            try
            {
                return await _context.DetailControls.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el detalle de control " + ex.Message);
            }
        }

        public async Task Agregar(DetailControl detalleControl)
        {
            try
            {
                await _context.DetailControls.AddAsync(detalleControl);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el detalle control " + ex.Message);
            }
        }

        public async Task AgregarEnCantidad(List<DetailControl> detallesControl)
        {
            try
            {
                List<DetailControl> detallesParaResumen = new List<DetailControl>();


                foreach (var detalle in detallesControl)
                {

                    if (detalle.Status == "notAvailable")
                    {
                        detallesParaResumen.Add(detalle);
                    }
                }

                if (detallesParaResumen.Any())
                {

                    var assetIds = detallesParaResumen.Select(d => d.IdActivo).Distinct().ToList();
                    var controlID = detallesParaResumen.FirstOrDefault().IdControl;

                    var assets = await _context.Actives
                        .Where(a => assetIds.Contains(a.Id))
                        .ToDictionaryAsync(a => a.Id, a => a);

                    var control = await _context.ControlRecords
                        .Include(c => c.IdSubsectorNavigation)
                        .FirstOrDefaultAsync(c => c.Id == controlID);

                    var mails = await _context.Users
                        .Where(u => u.IdCompany == control.IdCompany && u.Rol == "admin")
                        .Select(u => u.Email)
                        .ToListAsync();

                    Console.WriteLine("MAILS A ENVIAR:");

                    var auditorId = detallesParaResumen.FirstOrDefault().IdAuditor;
                    var Auditor = await _context.Users.FindAsync(auditorId);

                    var filasHtml = new StringBuilder();

                    foreach (var activo in detallesParaResumen)
                    {
                        var asset = assets.GetValueOrDefault(activo.IdActivo);

                        filasHtml.AppendLine($@"
                            <tr style=""border-bottom: 1px solid #f1f5f9;"">
                              <td style=""padding: 12px 10px; font-size: 13px; color: #4a5568;"">{asset.Name ?? "N/A"}</td>
                              <td style=""padding: 12px 10px; font-size: 13px; color: #4a5568;"">{Auditor.Name ?? "N/A"}</td>
                              <td style=""padding: 12px 10px; font-size: 13px; color: #4a5568;"">{asset.TagRfid ?? "N/A"}</td>
                            </tr>");
                    }

                    var fechaControl = DateTimeOffset.FromUnixTimeSeconds(control.Date)
                                                     .ToLocalTime() // convierte a tu zona horaria local
                                                     .DateTime;

                    var subSector = control.IdSubsectorNavigation.Name;

                    var body = $@"
                            <!DOCTYPE html>
                            <html lang=""es"">
                            <head>
                              <meta charset=""UTF-8"" />
                              <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
                              <title>Activos No Encontrados - Control de Inventario</title>
                            </head>
                            <body style=""margin:0; padding:0; background:#f5f7fa; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; color:#333;"">
                              <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""background:#f5f7fa; padding: 40px 0;"">
                                <tr>
                                  <td align=""center"">
                                    <table role=""presentation"" width=""650"" cellspacing=""0"" cellpadding=""0"" style=""background:#ffffff; border-radius:8px; box-shadow:0 2px 8px rgba(0,0,0,0.1); padding: 30px;"">
                                      <tr>
                                        <td style=""text-align:center; padding-bottom: 30px;"">
                                          <h1 style=""margin:0; font-weight:700; font-size:24px;"">Activos No Encontrados</h1>
                                          <h2 style=""margin:5px 0 0 0; font-weight:500; font-size:18px; color:#4a5568;"">Subsector: {subSector}</h2>
                                          <h2 style=""margin:5px 0 0 0; font-weight:500; font-size:18px; color:#4a5568;"">Fecha del control: {fechaControl.ToString("dd/MM/yyyy HH:mm")}</h2>
                                        </td>
                                      </tr>
                                      <tr>
                                        <td style=""padding-bottom: 25px;"">      
                                          <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""border: 1px solid #e2e8f0; border-radius: 6px; overflow: hidden;"">
                                            <thead>
                                              <tr style=""background:#f7fafc;"">
                                                <th style=""padding: 12px 10px; text-align: left; font-size: 13px; font-weight: 600; color: #2d3748; border-bottom: 1px solid #e2e8f0;"">Nombre del Activo</th>
                                                <th style=""padding: 12px 10px; text-align: left; font-size: 13px; font-weight: 600; color: #2d3748; border-bottom: 1px solid #e2e8f0;"">Auditor</th>
                                                <th style=""padding: 12px 10px; text-align: left; font-size: 13px; font-weight: 600; color: #2d3748; border-bottom: 1px solid #e2e8f0;"">Tag Rfid</th>
                                              </tr>
                                            </thead>
                                            <tbody>
                                              {filasHtml}
                                            </tbody>
                                          </table>
                                        </td>
                                      </tr>
                                      <tr>
                                        <td style=""font-size:14px; line-height:1.4; color:#a0aec0; border-top:1px solid #e2e8f0; padding-top:20px;"">
                                          Este es un mensaje automático generado por el sistema de control de inventario.<br />
                                          Para consultas adicionales, contacte al administrador del sistema.
                                        </td>
                                      </tr>
                                      <tr>
                                        <td style=""font-size:12px; color:#cbd5e0; padding-top: 15px; text-align:center;"">
                                          axAssetControl <br/>
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

                    await _sendMail.SendEmailtoCantity(
                        mails,
                        $"Activos No Encontrados - {subSector} ({detallesParaResumen.Count} faltantes)",
                        body
                    );
                }

                await _context.DetailControls.AddRangeAsync(detallesControl);
                await _context.SaveChangesAsync();

                Console.WriteLine("SE CREO Y SE ENVIO EL DETALLE CON EXITO");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);

                throw new Exception("Error al crear los detalles de control: " + ex.Message);
            }
        }

        public async Task Eliminar(int id)
        {
            try
            {
                var detalleControl = await _context.DetailControls.FindAsync(id);
                if (detalleControl != null)
                {
                    _context.DetailControls.Remove(detalleControl);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el detalle control " + ex.Message);
            }
        }

        public async Task Actualizar(DetailControl detalleControl)
        {
            try
            {
                _context.DetailControls.Update(detalleControl);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el detalle control en la base de datos " + ex.Message + " / " + ex.InnerException);
            }
        }
    }
}
