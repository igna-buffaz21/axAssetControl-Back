using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.SectorDTO;
using axAssetControl.Entidades.Dtos.SubSectorDTO;
using axAssetControl.Mapeo;
using Microsoft.EntityFrameworkCore;

namespace axAssetControl.AccesoDatos
{
    public class SubSectorAD
    {
        private readonly AxAssetControlDbContext _context;

        public SubSectorAD(AxAssetControlDbContext context)
        {
            _context = context;
        }

        public async Task<List<Subsector>> ObtenerTodos(int idsector, int idEmpresa, bool status)
        {
            try
            {
                return await _context.Subsectors.AsNoTracking().Where(s => s.IdSector == idsector && s.IdEmpresa == idEmpresa && s.Status == status).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los subsectores", ex);
            }
        }

        public async Task<Subsector> ObtenerPorId(int id)
        {
            try
            {
                return await _context.Subsectors.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el subsector " + ex.Message);
            }
        }

        public async Task Agregar(Subsector subSector)
        {
            try
            {
                await _context.Subsectors.AddAsync(subSector);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el subsector " + ex.Message);
            }
        }

        /*public async Task Eliminar(int id)
        {
            try
            {
                var subSector = await _context.Subsectors.FindAsync(id);
                if (subSector != null)
                {
                    _context.Subsectors.Remove(subSector);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el subsector " + ex.Message);
            }
        }*/

        public async Task CambiarEstado(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var subSector = await _context.Subsectors.FindAsync(id);

                if (subSector == null)
                {
                    throw new Exception("Locacion no encontrada");
                }

                bool estadoActivacion = subSector.Status; ///determinar si vamos a activar o desactivar ///si el estado actual es true, vamos a desactivar, por ende entra al if, y si es false entra al else y lo activa


                if (estadoActivacion) ///caso de desactivacion
                {
                    subSector.Status = false;

                        var activos = await _context.Actives
                            .Where(a => a.IdSubsector == subSector.Id && a.Status == true)
                            .ToListAsync();

                        foreach (var activo in activos)
                        {
                            activo.Status = false;
                        }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                else ///caso de activacion
                {
                    subSector.Status = true;

                        var activos = await _context.Actives
                            .Where(a => a.IdSubsector == subSector.Id && a.Status == false)
                            .ToListAsync();

                        foreach (var activo in activos)
                        {
                            activo.Status = true;
                        }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error al actualizar la locacion en la base de datos " + ex.Message);
            }
        }

        public async Task Actualizar(Subsector subSec)
        {
            try
            {
                var subSector = await _context.Subsectors.FindAsync(subSec.Id);

                if (subSector == null)
                {
                    throw new Exception("Subsector no encontrado");
                }

                subSector.Name = subSec.Name;
                subSector.TagRfid = subSec.TagRfid;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el subsector en la base de datos " + ex.Message);
            }
        }

        public async Task<List<ObtenerSubSectorDTO>> ObtenerSubSectorPorNombre(int idSector, string nombre)
        {
            try
            {
                var subSector = await _context.Subsectors
                    .Where(s => s.IdSector == idSector && s.Name.Contains(nombre)).ToListAsync();

                var subSectorDTO = MapeoSubSector.ObtenerSubsector(subSector);

                return subSectorDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los subsectores " + ex.Message);
            }
        }

        public async Task<List<ObtenerSubSectorDTO>> FiltrarSubSectores(int idSector, string orden)
        {
            try
            {

                var query = _context.Subsectors
                    .Where(s => s.IdSector == idSector)
                    .AsQueryable();

                if (orden.ToLower() == "asc")
                {
                    query = query.OrderBy(s => s.Name);
                }
                else if (orden.ToLower() == "desc")
                {
                    query = query.OrderByDescending(s => s.Name);
                }


                var subSectores = await query.ToListAsync();

                var subSectoresDTO = MapeoSubSector.ObtenerSubsector(subSectores);

                return subSectoresDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los subsectores " + ex.Message);
            }
        }

        public async Task<List<Active>> obtenerActivosDeSubsectorConRfid(string tagRfid)
        {
            try
            {
                var subSector = await _context.Subsectors.FirstOrDefaultAsync(ss => ss.TagRfid == tagRfid);

                if (subSector == null)
                {
                    throw new Exception("No se encontro ningun subsector");
                }

                var activos = await _context.Actives
                    .Where(a => a.IdSubsector == subSector.Id)
                    .ToListAsync();

                return activos;
            }
            catch(Exception ex)
            {
                throw new Exception("Error al intentar obtener los activos" + ex.Message);
            }
        }

        public async Task AsignarTagSubsector(string tagRfid, int idSubsector, int idEmpresa)
        {
            try
            {
                var subsector = await _context.Subsectors
                    .Where(ss => ss.Id == idSubsector && ss.IdEmpresa == idEmpresa)
                    .FirstOrDefaultAsync();

                if (subsector == null)
                {
                    throw new Exception("Subsector no encontrado");
                }

                subsector.TagRfid = tagRfid;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al asignar un tag RFID al subsector");
            }
        }

    }
}
