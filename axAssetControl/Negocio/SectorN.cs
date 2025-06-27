using axAssetControl.AccesoDatos;
using axAssetControl.Entidades;
using axAssetControl.Mapeo;
using axAssetControl.Entidades.Dtos.SectorDTO;
using axAssetControl.Entidades.Dtos.LocacionDTO;

namespace axAssetControl.Negocio
{
    public class SectorN
    {
        private readonly SectorAD _sectorAD;

        public SectorN(SectorAD sectorAD)
        {
            _sectorAD = sectorAD;
        }

        public async Task<List<Sector>> ObtenerSectores(int idsector, int idEmpresa, bool status)
        {
            return await _sectorAD.ObtenerTodos(idsector, idEmpresa, status);
        }

        public async Task<Sector> ObtenerSectorPorId(int id)
        {
            return await _sectorAD.ObtenerPorId(id);
        }

        public async Task CrearSector(CrearSectorDTO sectorDTO)
        {

            var sector = MapeoSector.CrearSector(sectorDTO);

            if (sector.IdLocation == 0)
            {
                throw new ArgumentException("El id de la locacion es obligatorio.");
            }//validacion id de locacion

            if (string.IsNullOrWhiteSpace(sector.Name))
            {
                throw new ArgumentException("El nombre de la empresa es obligatorio.");
            }///Validacion nombre empresa


            await _sectorAD.Agregar(sector);
        }

        /*public async Task EliminarSector(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("El id del sector es obligatorio.");
            }
            await _sectorAD.Eliminar(id);
        }*/

        public async Task ActualizarSector(ActualizarSectorDTO sectorDTO)
        {

            var sector = MapeoSector.ActualizarSector(sectorDTO);

            if (string.IsNullOrWhiteSpace(sector.Name))
            {
                throw new ArgumentException("El nombre de la empresa es obligatorio.");
            }///Validacion nombre empresa

            await _sectorAD.Actualizar(sector);

        }

        public async Task CambiarEstado(int id)
        {

            await _sectorAD.CambiarEstado(id);

        }

        public async Task<List<ObtenerSectorPorControlDTO>> ObtenerSectorPorNombre(int idLocacion, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("Nombre Invalido");
            }

            if (idLocacion == 0)
            {
                throw new ArgumentException("Id empresa invalido");
            }

            return await _sectorAD.ObtenerSectorPorNombre(idLocacion, nombre);
        }

        public async Task<List<ObtenerSectorPorControlDTO>> FiltarSectores(int idLocacion, string orden)
        {
            if (string.IsNullOrWhiteSpace(orden))
            {
                throw new ArgumentException("Orden Invalido");
            }

            if (idLocacion == 0)
            {
                throw new ArgumentException("Id empresa invalido");
            }

            return await _sectorAD.FiltrarSectores(idLocacion, orden);
        }
    }
}
