namespace axAssetControl.Entidades.Dtos.SubSectorDTO
{
    public class CrearSubSectorDTO
    {
        public int IdSector { get; set; }

        public string Name { get; set; } = null!;

        public string? TagRfid { get; set; } = null;

        public int IdEmpresa { get; set; }

    }
}
