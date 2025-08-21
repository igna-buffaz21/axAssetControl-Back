using axAssetControl.AccesoDatos;
using axAssetControl.Entidades.Dtos;
using axAssetControl.Entidades;
using axAssetControl.Mapeo;
using axAssetControl.Entidades.Dtos.RegistroControlDTO;

namespace axAssetControl.Negocio
{
    public class RegistroControlN
    {
        private readonly RegistroControlAD _registroControlAD;

        public RegistroControlN(RegistroControlAD registroControlAD)
        {
            _registroControlAD = registroControlAD;
        }

        public async Task<List<ObtenerHistorialDTO>> ObtenerRegistrosControl(int idSubSector)
        {
            return await _registroControlAD.ObtenerTodos(idSubSector);
        }

        public async Task<ControlRecord> ObtenerRegistrosControlPorId(int id)
        {
            return await _registroControlAD.ObtenerPorId(id);
        }

        public async Task<int> CrearRegistroControl(CrearRegistroControlDTO registroControlDTO)
        {

            var registroControl = MapeoRegistroControl.CrearRegistroControl(registroControlDTO);

            if (registroControl.IdSubsector == 0)
            {
                throw new ArgumentException("El id del subsector es obligatorio.");
            }//validacion id_subsector

            return await _registroControlAD.Agregar(registroControl);
        }

        public async Task EliminarRegistroControl(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("El id del RegistroControl es obligatorio.");
            }
            await _registroControlAD.Eliminar(id);
        }

        public async Task<ControlRecordDTO> ObtenerUltimoControl(int idSubSector)
        {
            if (idSubSector == 0)
            {
                throw new ArgumentException("Id subsector obligatorio!");
            }

            return await _registroControlAD.ObtenerUltimoControl(idSubSector);
        }

        /*public async Task<ControlRecordDTO> ObtenerUltimoControlPorNombreActivo(int idSubSector, string nombre)
        {
            if (idSubSector == 0)
            {
                throw new ArgumentException("Id subsector obligatorio!");
            }
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("Nombre invalido");
            }

            return await _registroControlAD.ObtenerUltimoControlPorNombreActivo(idSubSector, nombre);
        } */

        public async Task<ControlRecordDTO> ObtenerControlEspecifico(int id, int idCompany)
        {
            if (id == 0)
            {
                throw new ArgumentException("Id obligatorio!");
            }

            return await _registroControlAD.ObtenerControlEspecifico(id, idCompany);
        }

        public async Task<List<ControlRecordDTO>> ObtenerActivosPerdidos(int idCompany)
        {
            return await _registroControlAD.ObtenerUltimoControlActivosPerdidos(idCompany);
        }

        public async Task SincronizarControlesYDetalles(SincronizacionDTO sincronizacionDTO)
        {

            if (sincronizacionDTO.Controles.Count == 0)
            {
                throw new ArgumentException("No hay controles!");
            }

            if (sincronizacionDTO.Detalles.Count == 0)
            {
                throw new ArgumentException("No hay detalles");
            }

            var control = MapeoRegistroControl.CrearControlLocal(sincronizacionDTO.Controles);

            var detalle = MapeoRegistroControl.CrearDetalleControlLocal(sincronizacionDTO.Detalles);

            await _registroControlAD.SincronizarControlesYDetalles(control, detalle);

        }

        ///ver si se puede actualizar un registroControl
        /*public async Task ActualizarEmpresa(ActualizarEmpresaDTO empresaDTO)
        {

            var empresa = MapeoEmpresa.ActualizarEmpresa(empresaDTO);

            if (string.IsNullOrWhiteSpace(empresa.Name))
            {
                throw new ArgumentException("El nombre de la empresa es obligatorio.");
            }///Validacion nombre empresa

            if (string.IsNullOrWhiteSpace(empresa.Status))
            {
                throw new ArgumentException("El status de la empresa es obligatorio.");
            }///Validacion status empresa

            await _empresaAD.Agregar(empresa);

        }*/
    }
}
