using axAssetControl.Entidades.Dtos.ActivoDTO;
using axAssetControl.Entidades.Dtos.UsuarioDTO;

namespace axAssetControl.Entidades.Dtos.DetalleControlDTO
{
    public class ObtenerDetalleControlDTO
    {
        public int Id { get; set; }

        public string Status { get; set; } = null!;

        public virtual ObtenerActivoPorControlDTO IdActivoNavigation { get; set; } = null!; 

        public virtual ObtenerUsuarioPorControlDTO IdAuditorNavigation { get; set; } = null!; 

    }
}
