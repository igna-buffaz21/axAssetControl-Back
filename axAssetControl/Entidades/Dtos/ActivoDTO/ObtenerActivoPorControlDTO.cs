namespace axAssetControl.Entidades.Dtos.ActivoDTO
{
    public class ObtenerActivoPorControlDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Brand { get; set; } = null!;

        public string TagRfid { get; set; } = null!;
    }
}
