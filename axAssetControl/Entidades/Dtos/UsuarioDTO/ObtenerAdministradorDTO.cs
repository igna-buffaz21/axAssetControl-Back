using axAssetControl.Entidades.Dtos.EmpresaDTO;

namespace axAssetControl.Entidades.Dtos.UsuarioDTO
{
    public class ObtenerAdministradorDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Rol { get; set; } = null!;

        public string Status { get; set; } = null!;

        public virtual ObtenerEmpresaDTO IdCompanyNavigation { get; set; } = null!;
    }
}
