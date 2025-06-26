using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.LocacionDTO;

namespace axAssetControl.Mapeo
{
    public static class MapeoLocacion
    {
        public static Location CrearLocacion(CrearLocacionDTO locacionDTO)
        {
            return new Location
            {
                IdCompany = locacionDTO.IdCompany,
                Name = locacionDTO.Name
            };
        }

        public static Location ActualizarLocacion(ActualizarLocacionDTO locacionDTO)
        {
            return new Location
            {
                Id = locacionDTO.Id,
                Name = locacionDTO.Name
            };
        }

        public static List<ObtenerLocacionesDTO> ObtenerLocacionesDTO(List<Location> locaciones)
        {
            return locaciones.Select(l => new ObtenerLocacionesDTO
            {
                Id = l.Id,
                IdCompany = l.IdCompany,
                Name = l.Name
            }).ToList();
        }

        public static Location CambiarEstado(CambiarEstadoDTO locacionDTO)
        {
            return new Location
            {
                Id = locacionDTO.Id
            };
        }
    }
}
