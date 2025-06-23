using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.SectorDTO;

namespace axAssetControl.Mapeo
{
    public static class MapeoSector
    {
        public static Sector CrearSector(CrearSectorDTO sectorDTO)
        {
            return new Sector
            {
                IdLocation = sectorDTO.IdLocation,
                Name = sectorDTO.Name,
                IdEmpresa = sectorDTO.IdEmpresa,
            };
        }
        public static Sector ActualizarSector(ActualizarSectorDTO sectorDTO)
        {
            return new Sector
            {
                Id = sectorDTO.Id,
                Name = sectorDTO.Name,
            };
        }

        public static List<ObtenerSectorPorControlDTO> ObtenerSectorDTO(List<Sector> sectores)
        {
            return sectores.Select(s => new ObtenerSectorPorControlDTO
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }
    }
}
