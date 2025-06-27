using axAssetControl.AccesoDatos;
using axAssetControl.Entidades.Dtos;
using axAssetControl.Entidades;
using axAssetControl.Mapeo;
using axAssetControl.Entidades.Dtos.ActivoDTO;
using System;
using axAssetControl.Entidades.Dtos.SubSectorDTO;

namespace axAssetControl.Negocio
{
    public class ActivoN
    {
        private readonly ActivosAD _activoAD;

        public ActivoN(ActivosAD activoAD)
        {
            _activoAD = activoAD;
        }

        public async Task<List<Active>> ObtenerActivos(int idsubsector, int idEmpresa, bool status)
        {
            return await _activoAD.ObtenerTodos(idsubsector, idEmpresa, status);
        }

        public async Task<Active> ObtenerActivoPorId(int id)
        {
            return await _activoAD.ObtenerPorId(id);
        }

        public async Task CrearActivo(CrearActivoDTO activoDTO)
        {
            var cantidad = activoDTO.Cantity;

            if (activoDTO.IdSubsector == 0)
            {
                throw new ArgumentException("El id del subsector obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(activoDTO.Name))
            {
                throw new ArgumentException("El nombre del activo es obligatorio.");
            }///Validacion nombre activo

            if (string.IsNullOrWhiteSpace(activoDTO.TagRfid))
            {
                throw new ArgumentException("El tagRfid");
            }///ver

            if (activoDTO.IdActiveType == 0)
            {
                throw new ArgumentException("el tipo de activo es obligatorio");
            }///ver

            var listaActivos = new List<Active>();

            for (var i = 0; i < cantidad; i++)
            {
                var activo = MapeoActivo.CrearActivo(activoDTO);
                listaActivos.Add(activo);
            }

            await _activoAD.Agregar(listaActivos);

        }

        /*public async Task EliminarActivo(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("El id del activo es obligatorio.");
            }
            await _activoAD.Eliminar(id);
        }*/

        public async Task ActualizarActivo (ActualizarActivoDTO activoDTO)
        {

            var activo = MapeoActivo.ActualizarActivo(activoDTO);

            if (activo.Id == 0)
            {
                throw new ArgumentException("El id del activo es obligatorio.");
            }

            /*if (activo.IdSubsector == 0)
            {
                throw new ArgumentException("El id del subsector obligatorio.");
            }*/

            if (string.IsNullOrWhiteSpace(activo.Name))
            {
                throw new ArgumentException("El nombre del activo es obligatorio.");
            }///Validacion nombre activo

            if (string.IsNullOrWhiteSpace(activo.TagRfid))
            {
                throw new ArgumentException("El tagRfid");
            }///ver

            if (activo.IdActiveType == 0)
            {
                throw new ArgumentException("el tipo de activo es obligatorio");
            }///ver

            await _activoAD.Actualizar(activo);

        }

        public async Task CambiarEstado(int id)
        {

            await _activoAD.CambiarEstado(id);
            
        }

        public async Task<List<ObtenerActivoDTO>> ObtenerActivoPorNombre(int idSubSector, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("Nombre Invalido");
            }

            if (idSubSector == 0)
            {
                throw new ArgumentException("Id subsector invalido");
            }

            return await _activoAD.ObtenerActivoPorNombre(idSubSector, nombre); 
        }

        public async Task<List<ObtenerActivoDTO>> FiltarActivos(int idSubSector, string orden)
        {
            if (string.IsNullOrWhiteSpace(orden))
            {
                throw new ArgumentException("Orden Invalido");
            }

            if (idSubSector == 0)
            {
                throw new ArgumentException("Id subsector invalido");
            }

            return await _activoAD.FiltrarActivos(idSubSector, orden);
        }
    }
}
