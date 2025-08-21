namespace axAssetControl.Entidades.Dtos.RegistroControlDTO
{
    public class CrearDetalleControlLocalDTO
    {
        public int Id { get; set; }

        public int Id_control { get; set; }

        public int Id_activo { get; set; }

        public string Status { get; set; } = null!;

        public int Id_auditor { get; set; }
    }
}

/*

    val id: Int,
    val id_control: Int,
    val id_activo: Int,
    val status: String,
    val id_auditor: Int,
    val sync: Int,
    val name: String,

*/
