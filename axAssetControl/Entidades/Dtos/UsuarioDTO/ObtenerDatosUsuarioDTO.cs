using axAssetControl.Entidades.Dtos.EmpresaDTO;

namespace axAssetControl.Entidades.Dtos.UsuarioDTO
{
    public class ObtenerDatosUsuarioDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Rol { get; set; } = null!;

        public virtual ObtenerEmpresaDTO CompanyNavigation { get; set; } = null!;
    }
}
