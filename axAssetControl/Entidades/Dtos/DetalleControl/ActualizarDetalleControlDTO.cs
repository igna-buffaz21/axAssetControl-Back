namespace axAssetControl.Entidades.Dtos.DetalleControlDTO
{
    public class ActualizarDetalleControlDTO
    {
        public int Id { get; set; }

        public int IdControl { get; set; }

        public int IdActivo { get; set; }

        public string Status { get; set; } = null!;

        public int IdAuditor { get; set; }

    }
}
