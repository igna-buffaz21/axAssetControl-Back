using axAssetControl.Entidades.Dtos.DetalleControlDTO;
using axAssetControl.Entidades.Dtos.SubSectorDTO;

namespace axAssetControl.Entidades.Dtos.RegistroControlDTO
{
    public class CrearRegistroControlLocalDTO
    {
        public int Id { get; set; }

        public int Id_subsector { get; set; }

        public long Date { get; set; }

        public int Id_Company { get; set; }

    }
}


/*
 
    val id: Int,
    val id_subsector: Int,
    val date: Long,
    val name: String

*/
