namespace axAssetControl.Entidades.Dtos.UsuarioDTO
{
    public class CrearUsuarioDTO
    {
        public int IdCompany { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Rol { get; set; } = null!;

        public string Status { get; set; } = null!;

    }
}
