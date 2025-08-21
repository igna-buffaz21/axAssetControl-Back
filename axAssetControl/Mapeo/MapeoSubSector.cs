using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.SubSectorDTO;

namespace axAssetControl.Mapeo
{
    public static class MapeoSubSector
    {
        public static Subsector CrearSubSector(CrearSubSectorDTO subSectorDTO) 
        {
            return new Subsector
            {
                IdSector = subSectorDTO.IdSector,
                Name = subSectorDTO.Name,
                TagRfid = subSectorDTO.TagRfid,
                IdEmpresa = subSectorDTO.IdEmpresa,
            };
        }

        public static Subsector ActualizarSubSector(ActualizarSubSectorDTO subSectorDTO)
        {
            return new Subsector
            {
                Id = subSectorDTO.Id,
                Name = subSectorDTO.Name
            };
        }

        public static List<ObtenerSubSectorDTO> ObtenerSubsector(List<Subsector> subsectors)
        {
            return subsectors.Select(s => new ObtenerSubSectorDTO
            {
                Id = s.Id,
                Name = s.Name,
                TagRfid = s.TagRfid
            }).ToList();
        }

        public static ObtenerSubSectorDTO ObtenerSubsectorRfidDTO(Subsector s)
        {
            return new ObtenerSubSectorDTO
            {
                Id = s.Id,
                Name = s.Name,
                TagRfid = s.TagRfid
            };
        }
    }
}
