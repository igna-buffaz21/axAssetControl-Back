using axAssetControl.AccesoDatos;
using axAssetControl.Entidades.Dtos;
using axAssetControl.Entidades;
using axAssetControl.Mapeo;
using axAssetControl.Entidades.Dtos.SubSectorDTO;
using axAssetControl.Entidades.Dtos.SectorDTO;

namespace axAssetControl.Negocio
{
    public class SubSectorN
    {
        private readonly SubSectorAD _subSectorAD;

        public SubSectorN(SubSectorAD subSectorAD)
        {
            _subSectorAD = subSectorAD;
        }

        public async Task<List<Subsector>> ObtenerSubSectores(int idsector, int idEmpresa)
        {
            return await _subSectorAD.ObtenerTodos(idsector, idEmpresa);
        }

        public async Task<Subsector> ObtenerSubSectorPorId(int id)
        {
            return await _subSectorAD.ObtenerPorId(id);
        }

        public async Task CrearSubSector(CrearSubSectorDTO subSectorDTO)
        {

            var subSector = MapeoSubSector.CrearSubSector(subSectorDTO);


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

        public async Task EliminarSubSector(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("El id del subsector es obligatorio.");
            }
            await _subSectorAD.Eliminar(id);
        }
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

    }
}
