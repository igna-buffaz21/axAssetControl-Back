﻿namespace axAssetControl.Entidades
{
    public class EmailSettings
    {
        public string From { get; set; }
        public string DisplayName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EnableSSL { get; set; }

    }
}
