using axAssetControl.Entidades;
using Microsoft.EntityFrameworkCore;

namespace axAssetControl.AccesoDatos
{
    public class TipoActivoAD
    {
        private readonly AxAssetControlDbContext _context;

        public TipoActivoAD(AxAssetControlDbContext context)
        {
            _context = context;
        }
        public async Task<List<ActiveType>> ObtenerTodos()
        {
            try
            {
                return await _context.ActiveTypes.AsNoTracking().ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("Eror al obtener los tipos de activo " + ex.Message);
            }
        }

        public async Task<ActiveType> ObtenerPorID(int id)
        {
            try
            {
                return await _context.ActiveTypes.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Eror al obtener los tipos de activo " + ex.Message);
            }
        }
    }
}
