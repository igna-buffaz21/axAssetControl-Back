using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.ActivoDTO;

namespace axAssetControl.Mapeo
{
    public static class MapeoActivo
    {
        public static Active CrearActivo(CrearActivoDTO activoDTO)
        {
            return new Active
            {
                IdSubsector = activoDTO.IdSubsector,
                Name = activoDTO.Name,
                Brand = activoDTO.Brand,
                Model = activoDTO.Model,
                SeriaNumber = activoDTO.SeriaNumber,
                TagRfid = activoDTO.TagRfid,
                IdActiveType = activoDTO.IdActiveType,
                IdEmpresa = activoDTO.idEmpresa,
            };
        }

        public static Active ActualizarActivo(ActualizarActivoDTO activoDTO)
        {
            return new Active
            {
                Id = activoDTO.Id,
                //IdSubsector = activoDTO.IdSubsector,
                Name = activoDTO.Name,
                Brand = activoDTO.Brand,
                Model = activoDTO.Model,
                SeriaNumber = activoDTO.SeriaNumber,
                TagRfid = activoDTO.TagRfid,
                IdActiveType = activoDTO.IdActiveType
            };
        }

        public static List<ObtenerActivoDTO> ObtenerActivo(List<Active> activos)
        {
            return activos.Select(a => new ObtenerActivoDTO
            {
                Id = a.Id,
                IdSubsector = a.IdSubsector,
                Name = a.Name,
                Brand = a.Brand,
                Model = a.Model,
                SeriaNumber = a.SeriaNumber,
                TagRfid = a.TagRfid,
                IdActiveType = a.IdActiveType
            }).ToList();
        }
    }
}
