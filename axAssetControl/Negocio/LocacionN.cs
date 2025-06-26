using axAssetControl.AccesoDatos;
using axAssetControl.Entidades;
using axAssetControl.Mapeo;
using axAssetControl.Entidades.Dtos.LocacionDTO;

namespace axAssetControl.Negocio
{
    public class LocacionN
    {
        private readonly LocacionAD _locacionAD;

        public LocacionN(LocacionAD locacionAD)
        {
            _locacionAD = locacionAD;
        }

        public async Task<List<Location>> ObtenerLocaciones(int idcompany)
        {
            return await _locacionAD.ObtenerTodos(idcompany);
        }
        public async Task<Location> ObtenerLocacionPorId(int id)
        {
            return await _locacionAD.ObtenerPorId(id);
        }
        public async Task CrearLocacion(CrearLocacionDTO locacionDTO)
        {

            var locacion = MapeoLocacion.CrearLocacion(locacionDTO);

            if (locacion.IdCompany == 0)
            {
                throw new ArgumentException("El Id de la empresa es obligatorio obligatorio.");
            }///Validacion companyID

            if (string.IsNullOrWhiteSpace(locacion.Name))
            {
                throw new ArgumentException("El nombre del usuario es obligatorio.");
            }///Validacion nombre usuario


            await _locacionAD.Agregar(locacion);
        }
        /*public async Task EliminarLocacion(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("El id del usuario es obligatorio.");
            }
            await _locacionAD.Eliminar(id);
        }*/


        public async Task ActualizarLocacion(ActualizarLocacionDTO locacionDTO)
        {

            var locacion = MapeoLocacion.ActualizarLocacion(locacionDTO);

            if (string.IsNullOrWhiteSpace(locacion.Name))
            {
                throw new ArgumentException("El nombre del usuario es obligatorio.");
            }///Validacion nombre usuario

            await _locacionAD.Actualizar(locacion);

        }

        public async Task CambiarEstado(CambiarEstadoDTO locacionDTO)
        {

            var locacion = MapeoLocacion.CambiarEstado(locacionDTO);

            await _locacionAD.CambiarEstado(locacion);

        }

        public async Task<List<ObtenerLocacionesDTO>> ObtenerLocacionesPorNombre(int idCompany, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("Nombre Invalido");
            }

            if (idCompany == 0)
            {
                throw new ArgumentException("Id empresa invalido");
            }

            return await _locacionAD.ObtenerLocacionPorNombre(idCompany, nombre);
        }

        public async Task<List<ObtenerLocacionesDTO>> FiltarLocaciones(int idCompany, string orden)
        {
            if (string.IsNullOrWhiteSpace(orden))
            {
                throw new ArgumentException("Orden Invalido");
            }

            if (idCompany == 0)
            {
                throw new ArgumentException("Id empresa invalido");
            }

            return await _locacionAD.FiltrarLocaciones(idCompany, orden);
        }
    }
}
