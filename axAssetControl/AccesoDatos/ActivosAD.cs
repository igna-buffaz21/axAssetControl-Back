using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.ActivoDTO;
using axAssetControl.Entidades.Dtos.SubSectorDTO;
using axAssetControl.Mapeo;
using Microsoft.EntityFrameworkCore;

namespace axAssetControl.AccesoDatos
{
    public class ActivosAD
    {
        private readonly AxAssetControlDbContext _context;

        public ActivosAD(AxAssetControlDbContext context)
        {
            _context = context;
        }

        public async Task<List<Active>> ObtenerTodos(int idsubsector, int idEmpresa)
        {
            try
            {
                return await _context.Actives.AsNoTracking().Where(a => a.IdSubsector == idsubsector && a.IdEmpresa == idEmpresa).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los activos " + ex.Message);
            }
        }

        public async Task<Active> ObtenerPorId(int id)
        {
            try
            {
                return await _context.Actives.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el activo " + ex.Message);
            }
        }

        public async Task Agregar(List<Active> activos)
        {
            try
            {
                await _context.Actives.AddRangeAsync(activos);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el activo " + ex.Message);
            }
        }

        public async Task Eliminar(int id)
        {
            try
            {
                var activo = await _context.Actives.FindAsync(id);
                if (activo != null)
                {
                    _context.Actives.Remove(activo);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el activo " + ex.Message);
            }
        }

        public async Task Actualizar(Active dto)
        {
            try
            {
                var activo =  await _context.Actives.FindAsync(dto.Id);

                if (activo == null)
                {
                    throw new Exception("Activo no encontrado");
                }

                activo.Name = dto.Name;
                activo.Brand = dto.Brand;
                activo.Model = dto.Model;
                activo.SeriaNumber = dto.SeriaNumber;
                activo.TagRfid = dto.TagRfid;
                activo.IdActiveType = dto.IdActiveType;


                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el activo en la base de datos " + ex);
            }
        }

        public async Task<List<ObtenerActivoDTO>> ObtenerActivoPorNombre(int idSubSector, string nombre)
        {
            try
            {
                var activos = await _context.Actives
                    .Where(a => a.IdSubsector == idSubSector && a.Name.Contains(nombre)).ToListAsync();

                var activosDTO = MapeoActivo.ObtenerActivo(activos);

                return activosDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los activos " + ex.Message);
            }
        }

        public async Task<List<ObtenerActivoDTO>> FiltrarActivos(int idSubSector, string orden)
        {
            try
            {

                var query = _context.Actives
                    .Where(a => a.IdSubsector == idSubSector)
                    .AsQueryable();

                if (orden.ToLower() == "asc")
                {
                    query = query.OrderBy(s => s.Name);
                }
                else if (orden.ToLower() == "desc")
                {
                    query = query.OrderByDescending(s => s.Name);
                }


                var activos = await query.ToListAsync();

                var activosDTO = MapeoActivo.ObtenerActivo(activos);

                return activosDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los activos " + ex.Message);
            }
        }
    }
}
