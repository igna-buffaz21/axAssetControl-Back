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

        public async Task<List<Location>> ObtenerTodos(int idcompany)
        {
            try
            {
                return await _context.Locations.AsNoTracking().Where(l => l.IdCompany == idcompany).ToListAsync();
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

        public async Task Eliminar(int id)
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
        }

        public async Task Actualizar(Location loca)
        {
            try
            {
                var locacion = await _context.Locations.FindAsync(loca.Id);

                if (locacion == null)
                {
                    throw new Exception("Usuario no encontrado");
                }

                locacion.Name = loca.Name;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el usuario en la base de datos " + ex.Message);
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
