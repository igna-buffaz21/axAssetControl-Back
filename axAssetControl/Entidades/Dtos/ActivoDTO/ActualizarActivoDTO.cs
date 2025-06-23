namespace axAssetControl.Entidades.Dtos.ActivoDTO
{
    public class ActualizarActivoDTO
    {
        public int Id { get; set; }

        //public int IdSubsector { get; set; }

        public string Name { get; set; } = null!;

        public string Brand { get; set; } = null!;

        public string Model { get; set; } = null!;

        public string SeriaNumber { get; set; } = null!;

        public string TagRfid { get; set; } = null!;

        public int IdActiveType { get; set; }

    }
}
