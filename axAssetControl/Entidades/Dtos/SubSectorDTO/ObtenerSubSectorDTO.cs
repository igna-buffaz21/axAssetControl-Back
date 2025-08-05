using axAssetControl.Entidades.Dtos.SectorDTO;

namespace axAssetControl.Entidades.Dtos.SubSectorDTO
{
    public class ObtenerSubSectorDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? TagRfid { get; set; } = null;
    }
}
