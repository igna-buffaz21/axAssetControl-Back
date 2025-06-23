namespace axAssetControl.Entidades.Dtos.ActivoDTO
{
    public class CrearActivoDTO
    {
        public int IdSubsector { get; set; }

        public string Name { get; set; } = null!; 

        public string Brand { get; set; } = null!; //capaz null

        public string Model { get; set; } = null!; //capaz null

        public string SeriaNumber { get; set; } = null!; //capaz null

        public string TagRfid { get; set; } = null!;

        public int IdActiveType { get; set; }

        public int Cantity { get; set; }

        public int idEmpresa { get; set; }
    }
}
