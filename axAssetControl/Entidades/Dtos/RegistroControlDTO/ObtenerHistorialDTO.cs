namespace axAssetControl.Entidades.Dtos.RegistroControlDTO
{
    public class ObtenerHistorialDTO
    {
        public int Id { get; set; }

        public long Date { get; set; }

        public string Status { get; set; } = "inProcces";
    }
}
