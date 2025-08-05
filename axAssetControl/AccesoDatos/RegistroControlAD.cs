using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.RegistroControlDTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace axAssetControl.AccesoDatos
{
    public class RegistroControlAD
    {
        private readonly AxAssetControlDbContext _context;

        public RegistroControlAD(AxAssetControlDbContext context)
        {
            _context = context;
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
                Console.WriteLine($"ID: {registroControl.Id}");
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

    }
}
