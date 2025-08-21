using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.DetalleControlDTO;

namespace axAssetControl.Mapeo
{
    public static class MapeoDetalleControl
    {
        public static DetailControl CrearDetalleControl(CrearDetalleControlDTO detalleControlDTO)
        {
            return new DetailControl
            {
                IdControl = detalleControlDTO.IdControl,
                IdActivo = detalleControlDTO.IdActivo,
                Status = detalleControlDTO.Status,
                IdAuditor = detalleControlDTO.IdAuditor
            };
        }

        public static List<DetailControl> CrearDetallesDeControlEnCantidad(List<CrearDetalleControlDTO> detallesDeControlEnCantidadDTO)
        {
            return detallesDeControlEnCantidadDTO.Select(dc => new DetailControl
            {
                IdControl = dc.IdControl,
                IdActivo = dc.IdActivo,
                Status = dc.Status,
                IdAuditor = dc.IdAuditor
            }).ToList();
        }


        public static DetailControl ActualizarDetalleControl(ActualizarDetalleControlDTO detalleControlDTO)
        {
            return new DetailControl
            {
                Id = detalleControlDTO.Id, 
                IdControl = detalleControlDTO.IdControl,
                Status = detalleControlDTO.Status,
                IdActivo = detalleControlDTO.IdActivo,
                IdAuditor = detalleControlDTO.IdAuditor
            };
        }
    }
}
