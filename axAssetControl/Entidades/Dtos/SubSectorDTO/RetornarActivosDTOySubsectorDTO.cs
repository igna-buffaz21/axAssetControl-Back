using axAssetControl.Entidades.Dtos.ActivoDTO;

namespace axAssetControl.Entidades.Dtos.SubSectorDTO
{
    public class RetornarActivosDTOySubsectorDTO
    {
        public List<ObtenerActivoDTO> ActivosDTO { get; set; }
        public string Subsector { get; set; }
    }
}
