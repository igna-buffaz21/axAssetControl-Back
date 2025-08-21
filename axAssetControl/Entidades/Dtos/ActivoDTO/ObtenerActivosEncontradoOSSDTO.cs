using axAssetControl.Entidades.Dtos.SubSectorDTO;

namespace axAssetControl.Entidades.Dtos.ActivoDTO
{
    public class ObtenerActivosEncontradoOSSDTO
    {
        public int Id { get; set; }

        public int IdSubsector { get; set; }

        public string Name { get; set; } = null!;

        public string? TagRfid { get; set; } = null;

        public virtual ObtenerSubSectorDTO IdSubsectorNavigation { get; set; } = null!;

    }
}
