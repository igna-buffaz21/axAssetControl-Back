namespace axAssetControl.Entidades.Dtos.DetalleControlDTO
{
    public class CrearDetalleControlDTO
    {
        public int IdControl { get; set; }

        public int IdActivo { get; set; }

        public string Status { get; set; } = null!;

        public int IdAuditor { get; set; }

    }
}
