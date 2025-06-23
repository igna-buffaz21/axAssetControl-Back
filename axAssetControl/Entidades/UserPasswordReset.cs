namespace axAssetControl.Entidades
{
    public class UserPasswordReset
    {
        public int Id { get; set; }
        
        public int IdUser { get; set; }

        public string Token { get; set; }

        public long Expiracion { get; set; }

        public bool Used { get; set; }

        public virtual User User { get; set; }
    }
}
