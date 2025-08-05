using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos;
using axAssetControl.Entidades.Dtos.UsuarioDTO;
using axAssetControl.Mapeo;
using Microsoft.EntityFrameworkCore;

namespace axAssetControl.AccesoDatos
{
    public class UsuarioAD
    {
        private readonly AxAssetControlDbContext _context;

        public UsuarioAD(AxAssetControlDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> ObtenerTodos(int idCompany, string status)
        {
            try
            {
                return await _context.Users.AsNoTracking().Where(u => u.IdCompany == idCompany && u.Status == status).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los usuarios", ex);
            }
        }

        public async Task<User> ObtenerDatosUsuario(int id)
        {
            try
            {
                var datosUsuario = await _context.Users
                    .Include(u => u.IdCompanyNavigation)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (datosUsuario == null)
                {
                    throw new Exception("No se encontro al usuario con el id " + id);
                }

                return datosUsuario;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario", ex);
            }
        }

        public async Task Agregar(User usuario)
        {
            try
            {
                await _context.Users.AddAsync(usuario);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el usuario", ex);
            }
        }

        public async Task AgregarUsuariosEnCantidad(List<User> users) ///PRUEBA KOTLIN ///DESPUES BORRAR
        {
            try
            {
                await _context.Users.AddRangeAsync(users);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear los usuario en cantidad", ex);
            }
        } 

        public async Task Eliminar(int id)
        {
            try
            {
                var usuario = await _context.Users.FindAsync(id);
                if (usuario != null)
                {
                    _context.Users.Remove(usuario);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el usuario", ex);
            }
        }

        public async Task Actualizar(User user)
        {
            try
            {
                var usuario = await _context.Users.FindAsync(user.Id);

                if (usuario == null)
                {
                    throw new Exception("Usuario no encontrado");
                }

                usuario.Name = user.Name;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception("Error al actualizar el usuario en la base de datos " + ex.Message);
            }
        }

        public async Task BajaUsuario(int id) 
        {
            try
            {
                var usuario = await _context.Users.FindAsync(id);

                if (usuario == null)
                {
                    throw new Exception("Usuario no encontrado");
                }

                usuario.Status = "disabled";

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el usuario en la base de datos " + ex.Message);
            }
        }

        public async Task AltaUsuario(int id)
        {
            try
            {
                var usuario = await _context.Users.FindAsync(id);

                if (usuario == null)
                {
                    throw new Exception("Usuario no encontrado");
                }

                usuario.Status = "actived";

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el usuario en la base de datos " + ex.Message);
            }
        }

        public async Task<List<User>> ObtenerUsuarioPorNombre(string name, string status, int idCompany)
        {
            try
            {
               var usuarios = await _context.Users
               .Where(u => u.Name.Contains(name) && u.Status == status && u.IdCompany == idCompany)
               .ToListAsync();

                return usuarios;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario", ex);
            }
        }

        public async Task<List<User>> ObtenerAdministradoresEmpresa(string role, string status)
        {
            try
            {
                var usuarios = await _context.Users
                    .Where(u => u.Status == status && u.Rol == role)
                    .Include(c => c.IdCompanyNavigation)
                    .ToListAsync();

                return usuarios;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los usuarios", ex);
            }
        }
    }
}
