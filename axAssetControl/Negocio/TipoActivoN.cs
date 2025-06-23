using axAssetControl.AccesoDatos;
using axAssetControl.Entidades;

namespace axAssetControl.Negocio
{
    public class TipoActivoN
    {
        private readonly TipoActivoAD _tipoActivoAD;

        public TipoActivoN(TipoActivoAD tipoActivoAD)
        {
            _tipoActivoAD = tipoActivoAD;
        }

        public async Task<List<ActiveType>> ObtenerTipoActivo()
        {
            return await _tipoActivoAD.ObtenerTodos();
        }

        public async Task<ActiveType> ObtenerTipoActivoPorId(int id)
        {
            return await _tipoActivoAD.ObtenerPorID(id);
        }
    }
}
