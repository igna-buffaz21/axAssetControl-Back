﻿namespace axAssetControl.Entidades.Dtos.UsuarioDTO
{
    public class ObtenerUsuarioDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Rol { get; set; } = null!;

        public string Status { get; set; } = null!;
    }
}
