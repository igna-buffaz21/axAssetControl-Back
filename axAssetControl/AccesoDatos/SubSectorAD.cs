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

        public async Task<List<Subsector>> ObtenerTodos(int idsector, int idEmpresa)
        {
            try
            {
                return await _context.Subsectors.AsNoTracking().Where(s => s.IdSector == idsector && s.IdEmpresa == idEmpresa).ToListAsync();
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

        public async Task Eliminar(int id)
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
    }
}
