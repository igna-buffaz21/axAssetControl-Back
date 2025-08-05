using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.UsuarioDTO;

namespace axAssetControl.Mapeo
{
    public static class MapeoUsuario
    {
        public static User CrearUsuarioMAP(CrearUsuarioDTO usuarioDTO)
        {
            return new User
            {
                IdCompany = usuarioDTO.IdCompany,
                Name = usuarioDTO.Name,
                Email = usuarioDTO.Email,
                Password = usuarioDTO.Password,
                Rol = usuarioDTO.Rol,
                Status = usuarioDTO.Status
            };
        }

        public static User ActualizarNombreUsuarioMAP(ActualizarNombreUsuarioDTO usuarioDTO)
        {
            return new User
            {
                Id = usuarioDTO.Id,
                Name = usuarioDTO.Name
            };
        }

        public static List<ObtenerUsuarioDTO> ObtenerUsuariosMAP(List<User> usuarios)
        {
            return usuarios.Select(u => new ObtenerUsuarioDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Rol = u.Rol,
                Status = u.Status
            }).ToList();
        }

        public static User AltaBajaUsuarioDTO(BajaAltaUsuarioDTO usuarioDTO)
        {
            return new User
            {
                Id = usuarioDTO.Id,
                Status = usuarioDTO.Status
            };
        }

        public static List<ObtenerAdministradorDTO> ObtenerAdministradorMAP(List<User> usuarios)
        {
            return usuarios.Select(u => new ObtenerAdministradorDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Rol =u.Rol,
                Status = u.Status,
                IdCompanyNavigation = new Entidades.Dtos.EmpresaDTO.ObtenerEmpresaDTO
                {
                    Id = u.IdCompanyNavigation.Id,
                    Name = u.IdCompanyNavigation.Name,
                    Status = u.IdCompanyNavigation.Status
                },
            }).ToList();
        }

        public static UsuarioCambiarPasswordDTO CambiarContraseñaMAP(User user)
        {
            return new UsuarioCambiarPasswordDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public static ObtenerDatosUsuarioDTO ObtenerDatosUsuarioA(User user)
        {
            return new ObtenerDatosUsuarioDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Rol = user.Rol,
                CompanyNavigation = new Entidades.Dtos.EmpresaDTO.ObtenerEmpresaDTO
                {
                    Id = user.IdCompanyNavigation.Id,
                    Name = user.IdCompanyNavigation.Name,
                    Status = user.IdCompanyNavigation.Status
                }
            };
        }
    }
}
