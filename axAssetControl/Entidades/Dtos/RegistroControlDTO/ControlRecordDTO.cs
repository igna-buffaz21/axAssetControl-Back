using axAssetControl.Entidades.Dtos.DetalleControlDTO;
using axAssetControl.Entidades.Dtos.SubSectorDTO;

namespace axAssetControl.Entidades.Dtos.RegistroControlDTO
{
    public class ControlRecordDTO
    {
        public int Id { get; set; }

        public virtual ObtenerSubSectorPorControlDTO IdSubsectorNavigation { get; set; } = null!;

        public DateTime Date { get; set; }

        public ICollection<ObtenerDetalleControlDTO> DetailControls { get; set; } = new List<ObtenerDetalleControlDTO>();
    }
}
