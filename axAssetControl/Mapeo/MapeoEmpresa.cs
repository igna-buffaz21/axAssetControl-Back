using axAssetControl.Entidades;
using axAssetControl.Entidades.Dtos.EmpresaDTO;

namespace axAssetControl.Mapeo
{
    public static class MapeoEmpresa
    {
        public static Company CrearEmpresa(CrearEmpresaDTO empresa)
        {
            return new Company
            {
                Name = empresa.Name
            };
        }

        public static Company ActualizarEmpresa(ActualizarEmpresaDTO empresa)
        {
            return new Company
            {
                Id = empresa.Id,
                Name = empresa.Name
            };
        }
    }

}
