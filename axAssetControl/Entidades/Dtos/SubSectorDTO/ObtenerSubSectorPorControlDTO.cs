using axAssetControl.Entidades.Dtos.SectorDTO;

namespace axAssetControl.Entidades.Dtos.SubSectorDTO
{
    public class ObtenerSubSectorPorControlDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string TagRfid { get; set; } = null!;

        public virtual ObtenerSectorPorControlDTO IdSectorNavigation { get; set; } = null!;

    }
}
