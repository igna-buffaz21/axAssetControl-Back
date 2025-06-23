using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.DetalleControlDTO;
using axAssetControl.Entidades.Dtos.RegistroControlDTO;

namespace axAssetControl.Mapeo
{
    public static class MapeoRegistroControl
    {
        public static ControlRecord CrearRegistroControl(CrearRegistroControlDTO registroControlDTO)
        {
            return new ControlRecord
            {
                IdSubsector = registroControlDTO.IdSubsector,
                Date = registroControlDTO.Date,
                IdCompany = registroControlDTO.IdCompany
            };
        }

        public static ControlRecordDTO ObtenerRegistroYDetalle(ControlRecord controlRecord)
        {
            return new ControlRecordDTO
            {
                Id = controlRecord.Id,
                IdSubsectorNavigation = new Entidades.Dtos.SubSectorDTO.ObtenerSubSectorPorControlDTO
                {
                    Id = controlRecord.IdSubsectorNavigation.Id,
                    Name = controlRecord.IdSubsectorNavigation.Name,
                    TagRfid = controlRecord.IdSubsectorNavigation.TagRfid
                },
                Date = controlRecord.Date,
                DetailControls = controlRecord.DetailControls.Select(d => new ObtenerDetalleControlDTO
                {
                    Id = d.Id,
                    Status = d.Status,
                    IdActivoNavigation = new Entidades.Dtos.ActivoDTO.ObtenerActivoPorControlDTO
                    {
                        Id = d.IdActivoNavigation.Id,
                        Name = d.IdActivoNavigation.Name,
                        Brand = d.IdActivoNavigation.Brand,
                        TagRfid = d.IdActivoNavigation.TagRfid,
                    },
                    IdAuditorNavigation = new Entidades.Dtos.UsuarioDTO.ObtenerUsuarioPorControlDTO
                    {
                        Id = d.IdAuditorNavigation.Id,
                        Name = d.IdAuditorNavigation.Name,
                    }

                }).ToList()
            };
        }

        public static List<ObtenerHistorialDTO> ObtenerHistorial(List<ControlRecord> controlRecord)
        {
            return controlRecord.Select(c =>  new ObtenerHistorialDTO
            {
                Id = c.Id,
                Date = c.Date,
                Status = c.Status
            }).ToList();
        }

        public static List<ControlRecordDTO> ObtenerActivosPerdidos(List<ControlRecord> controlRecords)
        {
            return controlRecords.Select(c => new ControlRecordDTO
            {
                Id = c.Id,
                Date = c.Date,
                IdSubsectorNavigation = new Entidades.Dtos.SubSectorDTO.ObtenerSubSectorPorControlDTO
                {
                    Id = c.IdSubsectorNavigation.Id,
                    Name = c.IdSubsectorNavigation.Name,
                    TagRfid = c.IdSubsectorNavigation.TagRfid,
                    IdSectorNavigation = new Entidades.Dtos.SectorDTO.ObtenerSectorPorControlDTO
                    {
                        Id = c.IdSubsectorNavigation.IdSectorNavigation.Id,
                        Name = c.IdSubsectorNavigation.IdSectorNavigation.Name
                    }
                },
                DetailControls = c.DetailControls.Select(d => new ObtenerDetalleControlDTO
                {
                    Id = d.Id,
                    Status = d.Status,
                    IdActivoNavigation = new Entidades.Dtos.ActivoDTO.ObtenerActivoPorControlDTO
                    {
                        Id = d.IdActivoNavigation.Id,
                        Name = d.IdActivoNavigation.Name,
                        Brand = d.IdActivoNavigation.Brand,
                        TagRfid = d.IdActivoNavigation.TagRfid,
                    },
                    IdAuditorNavigation = new Entidades.Dtos.UsuarioDTO.ObtenerUsuarioPorControlDTO
                    {
                        Id = d.IdAuditorNavigation.Id,
                        Name = d.IdAuditorNavigation.Name,
                    }
                }).ToList()
            }).ToList();
        }
    }
}
