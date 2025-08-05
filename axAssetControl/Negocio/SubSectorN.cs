using axAssetControl.AccesoDatos;
using axAssetControl.Entidades.Dtos;
using axAssetControl.Entidades;
using axAssetControl.Mapeo;
using axAssetControl.Entidades.Dtos.SubSectorDTO;
using axAssetControl.Entidades.Dtos.SectorDTO;
using axAssetControl.Entidades.Dtos.ActivoDTO;
using Microsoft.IdentityModel.Tokens;

namespace axAssetControl.Negocio
{
    public class SubSectorN
    {
        private readonly SubSectorAD _subSectorAD;

        public SubSectorN(SubSectorAD subSectorAD)
        {
            _subSectorAD = subSectorAD;
        }

        public async Task<List<Subsector>> ObtenerSubSectores(int idsector, int idEmpresa, bool status)
        {
            return await _subSectorAD.ObtenerTodos(idsector, idEmpresa, status);
        }

        public async Task<Subsector> ObtenerSubSectorPorId(int id)
        {
            return await _subSectorAD.ObtenerPorId(id);
        }

        public async Task CrearSubSector(CrearSubSectorDTO subSectorDTO)
        {

            var subSector = MapeoSubSector.CrearSubSector(subSectorDTO);

            subSector.Status = true;


            if (string.IsNullOrWhiteSpace(subSector.Name))
            {
                throw new ArgumentException("El nombre del usuario es obligatorio.");
            }///Validacion nombre usuario

            if (subSector.IdSector == 0)
            {
                throw new ArgumentException("El id del sector es obligatorio.");
            }///validacion id de sector

            await _subSectorAD.Agregar(subSector);
        }

        /*public async Task EliminarSubSector(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("El id del subsector es obligatorio.");
            }
            await _subSectorAD.Eliminar(id);
        }*/

        public async Task ActualizarSubSector(ActualizarSubSectorDTO subSectorDTO)
        {

            var subSector = MapeoSubSector.ActualizarSubSector(subSectorDTO);

            if (string.IsNullOrWhiteSpace(subSector.Name))
            {
                throw new ArgumentException("El nombre del usuario es obligatorio.");
            }///Validacion nombre usuario

            if (subSector.Id == 0)
            {
                throw new ArgumentException("El id del subsector es obligatorio.");
            }

            await _subSectorAD.Actualizar(subSector);

        }

        public async Task CambiarEstado(int id)
        {

            await _subSectorAD.CambiarEstado(id);

        }

        public async Task<List<ObtenerSubSectorDTO>> ObtenerSubSectorPorNombre(int idSector, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("Nombre Invalido");
            }

            if (idSector == 0)
            {
                throw new ArgumentException("Id sector invalido");
            }

            return await _subSectorAD.ObtenerSubSectorPorNombre(idSector, nombre);
        }

        public async Task<List<ObtenerSubSectorDTO>> FiltarSubSectores(int idSector, string orden)
        {
            if (string.IsNullOrWhiteSpace(orden))
            {
                throw new ArgumentException("Orden Invalido");
            }

            if (idSector == 0)
            {
                throw new ArgumentException("Id sector invalido");
            }

            return await _subSectorAD.FiltrarSubSectores(idSector, orden);
        }

        public async Task<List<ObtenerActivoDTO>> obtenerActivosDeSubsectorConRfid(string tagRfid)
        {
            if (tagRfid == null)
            {
                throw new ArgumentException("tag nulo");
            }

            var activos = await _subSectorAD.obtenerActivosDeSubsectorConRfid(tagRfid);

            var activoDTO = MapeoActivo.ObtenerActivo(activos);

            return activoDTO;
        }

        public async Task AsignarTagSubsector(string tagRfid, int idSubsector, int idEmpresa)
        {
            if (tagRfid.IsNullOrEmpty())
            {
                throw new ArgumentException("El tag Rfid no puede ser nulo");
            }
            if (idSubsector == 0)
            {
                throw new ArgumentException("El id del Subsector no puede ser nulo");
            }
            if (idEmpresa == 0)
            {
                throw new ArgumentException("El id de la Empresa no puede ser nulo");
            }

            await _subSectorAD.AsignarTagSubsector(tagRfid, idSubsector, idEmpresa);
        }

    }
}
