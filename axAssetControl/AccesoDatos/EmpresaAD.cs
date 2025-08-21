using axAssetControl.Entidades;
using Microsoft.EntityFrameworkCore;

namespace axAssetControl.AccesoDatos
{
    public class EmpresaAD
    {
        private readonly AxAssetControlDbContext _context;

        public EmpresaAD(AxAssetControlDbContext context)
        {
            _context = context;
        }

        public async Task<List<Company>> ObtenerTodos(string status)
        {
            try
            {
                return await _context.Companies.AsNoTracking()
                    .Where(c => c.Status == status)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las empresas", ex);
            }
        }

        public async Task<Company> ObtenerPorId(int id)
        {
            try
            {
                return await _context.Companies.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la empresa", ex);
            }
        }

        public async Task Agregar(Company empresa)
        {
            try
            {
                await _context.Companies.AddAsync(empresa);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la empresa " + ex.Message);
            }
        }

        public async Task Eliminar(int id)
        {
            try
            {
                var empresa = await _context.Companies.FindAsync(id);
                if (empresa != null)
                {
                    _context.Companies.Remove(empresa);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la empresa", ex);
            }
        }

        public async Task Actualizar(Company empresa)
        {
            try
            {
                var empresaCompleta = await _context.Companies.FindAsync(empresa.Id);

                if (empresaCompleta == null)
                {
                    throw new Exception("Empresa no encontrada");
                }

                empresaCompleta.Name = empresa.Name;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la empresa en la base de datos", ex);
            }
        }

        public async Task AltaBajaEmpresa(int idEmpresa, string status)
        {
            try
            {
                var empresa = await _context.Companies.FindAsync(idEmpresa);

                if (empresa == null)
                {
                    throw new Exception("Empresa no encontrado");
                }

                empresa.Status = status;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la empresa en la base de datos", ex);
            }
        }

        public async Task<String> ObtenerNombreEmpresaPorId(int id)
        {
            try
            {
                var nombreEmpresa = await _context.Companies
                    .Where(c => c.Id == id)
                    .Select(c => c.Name)
                    .FirstOrDefaultAsync();

                if (nombreEmpresa == null)
                {
                    throw new ArgumentException("Id de empresa no valido");
                }

                return nombreEmpresa;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la empresa", ex);
            }
        }
    }
}

