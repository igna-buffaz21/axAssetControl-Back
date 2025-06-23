using axAssetControl.AccesoDatos;
using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.EmpresaDTO;
using axAssetControl.Mapeo;

namespace axAssetControl.Negocio
{
    public class EmpresaN
    {
        private readonly EmpresaAD _empresaAD;

        public EmpresaN(EmpresaAD empresaAD)
        {
            _empresaAD = empresaAD;
        }

        public async Task<List<Company>> ObtenerEmpresas(bool mostrarInactivos)
        {
            string s = "actived";

            if (mostrarInactivos)
            {
                s = "disabled";
            }


            return await _empresaAD.ObtenerTodos(s);
        }

        public async Task<Company> ObtenerEmpresaPorId(int id)
        {
            return await _empresaAD.ObtenerPorId(id);
        }

        public async Task CrearEmpresa(CrearEmpresaDTO empresaDTO)
        {

            var empresa = MapeoEmpresa.CrearEmpresa(empresaDTO);

            if (string.IsNullOrWhiteSpace(empresa.Name))
            {
                throw new ArgumentException("El nombre de la empresa es obligatorio.");
            }///Validacion nombre empresa

            empresa.Status = "actived";

            await _empresaAD.Agregar(empresa);
        }

        public async Task EliminarEmpresa(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("El id de la empresa es obligatorio.");
            }
            await _empresaAD.Eliminar(id);
        }

        public async Task ActualizarEmpresa(ActualizarEmpresaDTO empresaDTO)
        {

            var empresa = MapeoEmpresa.ActualizarEmpresa(empresaDTO);

            if (string.IsNullOrWhiteSpace(empresa.Name))
            {
                throw new ArgumentException("El nombre de la empresa es obligatorio.");
            }///Validacion nombre empresa


            await _empresaAD.Actualizar(empresa);

        }

        public async Task AltaBajaEmpresa(ActualizarEstadoEmpresaDTO empresaDTO)
        {

            if (empresaDTO.Id == 0)
            {
                throw new ArgumentException("Id Invalido");
            }

            if (empresaDTO.Status)
            {
                string s = "actived";
                await _empresaAD.AltaBajaEmpresa(empresaDTO.Id, s);
            }
            else if (!empresaDTO.Status)
            {
                string s = "disabled";
                await _empresaAD.AltaBajaEmpresa(empresaDTO.Id, s);
            }
            else
            {
                throw new ArgumentException("Estado no valido");
            }
        }
    }
}
