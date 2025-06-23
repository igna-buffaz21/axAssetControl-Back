using axAssetControl.Entidades;
using Microsoft.EntityFrameworkCore;

namespace axAssetControl.AccesoDatos
{
    public class DetalleControlAD
    {
        private readonly AxAssetControlDbContext _context;

        public DetalleControlAD(AxAssetControlDbContext context)
        {
            _context = context;
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
