using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.RegistroControlDTO;
using axAssetControl.Negocio;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace axAssetControl.AccesoDatos
{
    public class RegistroControlAD
    {
        private readonly AxAssetControlDbContext _context;
        private readonly SendMail _sendMail;


        public RegistroControlAD(AxAssetControlDbContext context, SendMail sendMail)
        {
            _context = context;
            _sendMail = sendMail;
        }

        public async Task<List<ObtenerHistorialDTO>> ObtenerTodos(int idSubSector)
        {
            try
            {
                var historial = await _context.ControlRecords.AsNoTracking().Where(c => c.IdSubsector == idSubSector).ToListAsync();

                if (historial == null)
                {
                    throw new ArgumentException("No se encontro ningun historial en el subsector " + idSubSector);
                }

                var historialDTO = Mapeo.MapeoRegistroControl.ObtenerHistorial(historial);

                return historialDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los RegistroControl " + ex.Message);
            }
        }

        public async Task<ControlRecord> ObtenerPorId(int id)
        {
            try
            {
                return await _context.ControlRecords.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los RegistroControl " + ex.Message);
            }
        }

        public async Task<int> Agregar(ControlRecord registroControl)
        {
            try
            {
                Console.WriteLine($"ID DEL CONTROL: {registroControl.Id}");

                Console.WriteLine($"STATUS DEL CONTROL: {registroControl.Status}");

                registroControl.Status = "Completed";

                await _context.ControlRecords.AddAsync(registroControl);

                await _context.SaveChangesAsync();

                return registroControl.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el RegistroControl " + ex.Message + " / " + ex.InnerException);
            }
        }

        public async Task Eliminar(int id)
        {
            try
            {
                var registroControl = await _context.ControlRecords.FindAsync(id);
                if (registroControl != null)
                {
                    _context.ControlRecords.Remove(registroControl);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el RegistroControl " + ex.Message);
            }
        }

        public async Task Actualizar(ControlRecord registroControl)
        {
            try
            {
                _context.ControlRecords.Update(registroControl);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el RegistroControl en la base de datos " + ex.Message);
            }
        }

        public async Task<ControlRecordDTO> ObtenerUltimoControl(int idSubSector)
        {
            try
            {
                var query = _context.ControlRecords
                    .Where(c => c.IdSubsector == idSubSector)
                    .OrderByDescending(c => c.Id) ///se trae el ultmo id(ultimo registro en teoria)

                    .Include(c => c.DetailControls).ThenInclude(dc => dc.IdActivoNavigation)
                    .Include(c => c.DetailControls).ThenInclude(dc => dc.IdAuditorNavigation)
                    .Include(c => c.IdSubsectorNavigation)
                        .ThenInclude(dss => dss.IdSectorNavigation);

                var sql = query.ToQueryString();
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("CONSULTA SQL " + sql);
                Console.WriteLine("------------------------------------------------");

                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                var ultimoControl = await query.FirstOrDefaultAsync();
                stopwatch.Stop();

                Console.WriteLine("------------------------------------------------");
                Console.WriteLine($"TIEMPO EJECUCION {stopwatch.ElapsedMilliseconds} ms");
                Console.WriteLine("------------------------------------------------");

                if (ultimoControl == null)
                {
                    return null;
                }

                var ultimoControlDTO = Mapeo.MapeoRegistroControl.ObtenerRegistroYDetalle(ultimoControl);

                return ultimoControlDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el ultimo registro control " + ex.Message);
            }
        }

        /*public async Task<ControlRecordDTO> ObtenerUltimoControlPorNombreActivo(int idSubSector, string nombre)
        {
            try
            {
                Console.WriteLine("******************************************************");

                var ultimoControl = await _context.ControlRecords
                    .Where(c => c.IdSubsector == idSubSector)
                    .OrderByDescending(c => c.Id) ///se trae el ultmo id(ultimo registro en teoria)
                    .Include(c => c.DetailControls).ThenInclude(dc => dc.IdActivoNavigation)
                    .Include(c => c.DetailControls).ThenInclude(dc => dc.IdAuditorNavigation)
                    .Include(c => c.IdSubsectorNavigation)
                        .ThenInclude(dss => dss.IdSectorNavigation)
                    .FirstOrDefaultAsync();

                if (ultimoControl == null)
                {
                    return null;
                }

                ultimoControl.DetailControls = ultimoControl.DetailControls
                .Where(dc => dc.IdActivoNavigation.Name.Contains(nombre, StringComparison.OrdinalIgnoreCase))
                .ToList();

                var ultimoControlDTO = Mapeo.MapeoRegistroControl.ObtenerRegistroYDetalle(ultimoControl);

                return ultimoControlDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el ultimo registro control " + ex.Message);
            }
        } */

        public async Task<ControlRecordDTO> ObtenerControlEspecifico(int id, int idCompany)
        {
            try
            {
                var control = await _context.ControlRecords
                    .Where(c => c.Id == id && c.IdCompany == idCompany)
                    .Include(c => c.DetailControls).ThenInclude(dc => dc.IdActivoNavigation)
                    .Include(c => c.DetailControls).ThenInclude(dc => dc.IdAuditorNavigation)
                    .Include(c => c.IdSubsectorNavigation)
                        .ThenInclude(dss => dss.IdSectorNavigation)

                    .FirstOrDefaultAsync();

                if (control == null)
                {
                    throw new ArgumentException("No se encontro el control " + id);
                }

                var controlDTO = Mapeo.MapeoRegistroControl.ObtenerRegistroYDetalle(control);

                return controlDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el control " + ex.Message);
            }
        }

        public async Task<List<ControlRecordDTO>> ObtenerUltimoControlActivosPerdidos(int idCompany)
        {
            try
            {
                var ultimosControlesIds = await _context.ControlRecords
                    .Where(c => c.IdCompany == idCompany)
                    .GroupBy(c => c.IdSubsector)
                    .Select(g => g.OrderByDescending(c => c.Id).First().Id)
                    .ToListAsync();

                var ultimosControles = await _context.ControlRecords
                    .Where(c => ultimosControlesIds.Contains(c.Id))
                    .Include(c => c.DetailControls)
                        .ThenInclude(dc => dc.IdActivoNavigation)
                    .Include(c => c.DetailControls)
                        .ThenInclude(dc => dc.IdAuditorNavigation)
                    .Include(c => c.IdSubsectorNavigation)
                        .ThenInclude(dss => dss.IdSectorNavigation)
                    .ToListAsync();

                if (ultimosControles == null)
                {
                    return null;
                }

                foreach (var control in ultimosControles)
                {
                    control.DetailControls = control.DetailControls
                        .Where(dc => dc.Status == "notAvailable")
                        .ToList();
                }

                var ultimosControlesDTO = Mapeo.MapeoRegistroControl.ObtenerActivosPerdidos(ultimosControles);

                return ultimosControlesDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el ultimo registro control " + ex.Message);
            }
        }

        public async Task SincronizarControlesYDetalles(List<ControlRecord> controlRecord, List<DetailControl> detailControl)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                List<DetailControl> detalles = new List<DetailControl>();
                List<DetailControl> detallesParaResumen = new List<DetailControl>();

                foreach (var control in controlRecord)
                {
                    var id = control.Id;

                    foreach (var detalle in detailControl)
                    {
                        if (detalle.IdControl == id)
                        {
                            detalles.Add(detalle);
                        }
                    }

                    control.Id = 0;

                    control.IdCompany = controlRecord[0].IdCompany;

                    control.Status = "Completed";

                    await _context.ControlRecords.AddAsync(control);
                    await _context.SaveChangesAsync(); 

                    var idControl = control.Id; 

                    foreach (var detalle in detalles)
                    {
                        if (detalle.Status == "notAvailable")
                        {
                            detallesParaResumen.Add(detalle);
                        }

                        detalle.Id = 0;
                        detalle.IdControl = idControl;
                    }

                    if (detallesParaResumen.Any())
                    {

                        var assetIds = detallesParaResumen.Select(d => d.IdActivo).Distinct().ToList();
                        var controlID = detallesParaResumen.FirstOrDefault().IdControl;

                        var assets = await _context.Actives
                            .Where(a => assetIds.Contains(a.Id))
                            .ToDictionaryAsync(a => a.Id, a => a);

                        var controlBD = await _context.ControlRecords
                            .Include(c => c.IdSubsectorNavigation)
                            .FirstOrDefaultAsync(c => c.Id == controlID);

                        var mails = await _context.Users
                            .Where(u => u.IdCompany == control.IdCompany && u.Rol == "admin")
                            .Select(u => u.Email)
                            .ToListAsync();

                        foreach (var mail in mails)
                        {
                            Console.WriteLine(mail);
                        }

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

                        var fechaControl = DateTimeOffset.FromUnixTimeSeconds(controlBD.Date)
                                                         .ToLocalTime() // convierte a tu zona horaria local
                                                         .DateTime;

                        var subSector = controlBD.IdSubsectorNavigation.Name;

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

                    await _context.DetailControls.AddRangeAsync(detalles);
                    await _context.SaveChangesAsync();

                    detalles.Clear();
                    detallesParaResumen.Clear();
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }


    }
}
