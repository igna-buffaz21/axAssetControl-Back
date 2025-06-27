using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.LocacionDTO;
using axAssetControl.Entidades.Dtos.SectorDTO;
using axAssetControl.Mapeo;
using Microsoft.EntityFrameworkCore;

namespace axAssetControl.AccesoDatos
{
    public class SectorAD
    {
        private readonly AxAssetControlDbContext _context;

        public SectorAD(AxAssetControlDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sector>> ObtenerTodos(int idlocacion, int idEmpresa, bool status)
        {
            try
            {
                return await _context.Sectors.AsNoTracking().Where(s => s.IdLocation == idlocacion && s.IdEmpresa == idEmpresa && s.Status == status).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los sectores " + ex.Message);
            }
        }

        public async Task<Sector> ObtenerPorId(int id)
        {
            try
            {
                return await _context.Sectors.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el sector " + ex.Message);
            }
        }

        public async Task Agregar(Sector sector)
        {
            try
            {
                await _context.Sectors.AddAsync(sector);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el sector " + ex.Message + "/" + ex.InnerException);
            }
        }

        /*public async Task Eliminar(int id)
        {
            try
            {
                var sector = await _context.Sectors.FindAsync(id);
                if (sector != null)
                {
                    _context.Sectors.Remove(sector);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el sector " + ex.Message);
            }
        }*/

        public async Task Actualizar(Sector sect)
        {
            try
            {
                var sector = await _context.Sectors.FindAsync(sect.Id);

                if (sector == null)
                {
                    throw new Exception("Sector no encontrado");
                }

                sector.Name = sect.Name;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el Sector en la base de datos " + ex.Message);
            }
        }

        public async Task CambiarEstado(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var sector = await _context.Sectors.FindAsync(id);

                if (sector == null)
                {
                    throw new Exception("Locacion no encontrada");
                }

                bool estadoActivacion = sector.Status; ///determinar si vamos a activar o desactivar ///si el estado actual es true, vamos a desactivar, por ende entra al if, y si es false entra al else y lo activa


                if (estadoActivacion) ///caso de desactivacion
                {
                    sector.Status = false;

                    var subsectores = await _context.Subsectors
                        .Where(ss => ss.IdSector == sector.Id && ss.Status == true)
                        .ToListAsync();

                    foreach (var subsector in subsectores)
                    {
                        subsector.Status = false;

                        var activos = await _context.Actives
                            .Where(a => a.IdSubsector == subsector.Id && a.Status == true)
                            .ToListAsync();

                        foreach (var activo in activos)
                        {
                            activo.Status = false;
                        }
                    }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
                else ///caso de activacion
                {
                    sector.Status = true;

                        var subsectores = await _context.Subsectors
                            .Where(ss => ss.IdSector == sector.Id && ss.Status == false)
                            .ToListAsync();

                        foreach (var subsector in subsectores)
                        {
                            subsector.Status = true;

                            var activos = await _context.Actives
                                .Where(a => a.IdSubsector == subsector.Id && a.Status == false)
                                .ToListAsync();

                            foreach (var activo in activos)
                            {
                                activo.Status = true;
                            }
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

        public async Task<List<ObtenerSectorPorControlDTO>> ObtenerSectorPorNombre(int idLocacion, string nombre)
        {
            try
            {
                var sector = await _context.Sectors
                    .Where(s => s.IdLocation == idLocacion && s.Name.Contains(nombre)).ToListAsync();

                var sectorDTO = MapeoSector.ObtenerSectorDTO(sector);

                return sectorDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los sectores " + ex.Message);
            }
        }

        public async Task<List<ObtenerSectorPorControlDTO>> FiltrarSectores(int idLocacion, string orden)
        {
            try
            {

                var query = _context.Sectors
                    .Where(s => s.IdLocation == idLocacion)
                    .AsQueryable();

                if (orden.ToLower() == "asc")
                {
                    query = query.OrderBy(s => s.Name);
                }
                else if (orden.ToLower() == "desc")
                {
                    query = query.OrderByDescending(s => s.Name);
                }


                var sectores = await query.ToListAsync();

                var sectoresDTO = MapeoSector.ObtenerSectorDTO(sectores);

                return sectoresDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los sectores " + ex.Message);
            }
        }
    }
}
