using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.ActivoDTO;
using axAssetControl.Entidades.Dtos.SubSectorDTO;

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

        public static ObtenerActivoDTO ObtenerActivoPorRfid(Active activo)
        {
            return new ObtenerActivoDTO
            {

                Id = activo.Id,
                IdSubsector = activo.IdSubsector,
                Name = activo.Name,
                Brand = activo.Brand,
                Model = activo.Model,
                SeriaNumber = activo.SeriaNumber,
                TagRfid = activo.TagRfid,
                IdActiveType = activo.IdActiveType
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

        public static List<ObtenerActivosEncontradoOSSDTO> ObtenerActivoDeOtroSubSector(List<Active> activos)
        {
            return activos.Select(a => new ObtenerActivosEncontradoOSSDTO
            {
                Id = a.Id,
                IdSubsector = a.IdSubsector,
                Name = a.Name,
                TagRfid = a.TagRfid,
                IdSubsectorNavigation = new ObtenerSubSectorDTO
                {
                    Id = a.IdSubsectorNavigation.Id,
                    Name = a.IdSubsectorNavigation.Name,
                    TagRfid = a.IdSubsectorNavigation.TagRfid
                }
            }).ToList();
        }
    }
}


/*
  
        private var idCompany: Int = 0

        private var idUsuario: Int = 0


        if (tokenManager.getCompanyId() != null && tokenManager.obtenerIdUsuario() != null) {
            idCompany = tokenManager.getCompanyId()!!
            idUsuario = tokenManager.obtenerIdUsuario()!!
        }


*/
