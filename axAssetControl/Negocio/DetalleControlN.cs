using axAssetControl.AccesoDatos;
using axAssetControl.Entidades.Dtos;
using axAssetControl.Entidades;
using axAssetControl.Mapeo;
using axAssetControl.Entidades.Dtos.DetalleControlDTO;

namespace axAssetControl.Negocio
{
    public class DetalleControlN
    {
        private readonly DetalleControlAD _detalleControlAD;

        public DetalleControlN(DetalleControlAD detalleControlAD)
        {
            _detalleControlAD = detalleControlAD;
        }

        public async Task<List<DetailControl>> ObtenerDetalleControl()
        {
            return await _detalleControlAD.ObtenerTodos();
        }

        public async Task<DetailControl> ObtenerDetalleControlPorId(int id)
        {
            return await _detalleControlAD.ObtenerPorId(id);
        }

        public async Task CrearDetalleControl(CrearDetalleControlDTO detalleControlDTO)
        {

            var detalleControl = MapeoDetalleControl.CrearDetalleControl(detalleControlDTO);

            if (detalleControl.IdControl == 0)
            {
                throw new Exception("El id control es obligatorio");
            }

            if (detalleControl.IdAuditor == 0)
            {
                throw new Exception("El id_auditor es obligatorio");
            }

            if (detalleControl.IdActivo == 0)
            {
                throw new Exception("El id_activo es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(detalleControl.Status))
            {
                throw new ArgumentException("El status es obligatorio.");
            }

            await _detalleControlAD.Agregar(detalleControl);
        }

        public async Task EliminarDetalleControl(int id) ////QUEDE ACA
        {
            if (id == 0)
            {
                throw new ArgumentException("El id del detalle control es obligatorio.");
            }
            await _detalleControlAD.Eliminar(id);
        }

        public async Task ActualizarDetalleControl(ActualizarDetalleControlDTO detalleControlDTO)
        {

            var detalleControl = MapeoDetalleControl.ActualizarDetalleControl(detalleControlDTO);

            if (detalleControl.Id == 0)
            {
                throw new Exception("Id es obligatorio!");
            }

            if (detalleControl.IdControl == 0)
            {
                throw new Exception("Id_control es obligatorio!");
            }

            if (detalleControl.IdActivo == 0)
            {
                throw new Exception("Id_activo es obligatorio!");
            }

            if (detalleControl.IdAuditor == 0)
            {
                throw new Exception("Id_auditor es obligatorio!");
            }

            if (string.IsNullOrWhiteSpace(detalleControl.Status))
            {
                throw new ArgumentException("El status del detalleControl es obligatorio.");
            }///Validacion status

            await _detalleControlAD.Actualizar(detalleControl);

        }
    }
}
