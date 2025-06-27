using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.LocacionDTO;
using axAssetControl.Mapeo;
using Microsoft.EntityFrameworkCore;

namespace axAssetControl.AccesoDatos
{
    public class LocacionAD
    {
        private readonly AxAssetControlDbContext _context;

        public LocacionAD(AxAssetControlDbContext context)
        {
            _context = context;
        }

        public async Task<List<Location>> ObtenerTodos(int idcompany, bool status)
        {
            try
            {
                return await _context.Locations.AsNoTracking().Where(l => l.IdCompany == idcompany && l.Status == status).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las locaciones " + ex.Message);
            }
        }

        public async Task<Location> ObtenerPorId(int id)
        {
            try
            {
                return await _context.Locations.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario " + ex.Message);
            }
        }

        public async Task Agregar(Location locacion)
        {
            try
            {
                await _context.Locations.AddAsync(locacion);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el usuario " + ex.Message);
            }
        }

        /*public async Task Eliminar(int id)
        {
            try
            {
                var locacion = await _context.Locations.FindAsync(id);
                if (locacion != null)
                {
                    _context.Locations.Remove(locacion);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la locacion" + ex.Message + " / " + ex.InnerException);
            }
        }*/ 



        public async Task Actualizar(Location loca)
        {
            try
            {
                var locacion = await _context.Locations.FindAsync(loca.Id);

                if (locacion == null)
                {
                    throw new Exception("Locacion no encontrado");
                }

                locacion.Name = loca.Name;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la Locacion en la base de datos " + ex.Message);
            }
        }

        public async Task CambiarEstado(Location loca)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var locacion = await _context.Locations.FindAsync(loca.Id);

                if (locacion == null)
                {
                    throw new Exception("Locacion no encontrada");
                }

                bool estadoActivacion = locacion.Status; ///determinar si vamos a activar o desactivar ///si el estado actual es true, vamos a desactivar, por ende entra al if, y si es false entra al else y lo activa
                ///

                if (estadoActivacion) ///caso de desactivacion
                {
                    locacion.Status = false;

                    var sectores = await _context.Sectors
                        .Where(s => s.IdLocation == loca.Id && s.Status == true)
                        .ToListAsync();

                    foreach (var sector in sectores)
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
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                else ///caso de activacion
                {
                    locacion.Status = true; 

                    var sectores = await _context.Sectors
                        .Where(s => s.IdLocation == loca.Id && s.Status == false)
                        .ToListAsync();

                    foreach (var sector in sectores)
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

        public async Task<List<ObtenerLocacionesDTO>> ObtenerLocacionPorNombre(int idcompany, string nombre)
        {
            try
            {
                var locacion = await _context.Locations
                    .Where(l => l.IdCompany == idcompany && l.Name.Contains(nombre)).ToListAsync();

                var locacionDTO = MapeoLocacion.ObtenerLocacionesDTO(locacion);

                return locacionDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las locaciones " + ex.Message);
            }
        }

        public async Task<List<ObtenerLocacionesDTO>> FiltrarLocaciones(int idCompany, string orden)
        {
            try
            {

                var query = _context.Locations
                    .Where(l => l.IdCompany == idCompany)
                    .AsQueryable();

                if (orden.ToLower() == "asc")
                {
                    query = query.OrderBy(l => l.Name);
                }
                else if (orden.ToLower() == "desc")
                {
                    query = query.OrderByDescending(l => l.Name);
                }


                var locaciones = await query.ToListAsync();

                var locacionesDTO = Mapeo.MapeoLocacion.ObtenerLocacionesDTO(locaciones);

                return locacionesDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las locaciones " + ex.Message);
            }
        }
    }
}
