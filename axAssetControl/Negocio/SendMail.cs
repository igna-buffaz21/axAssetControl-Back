﻿using System.Net;
using System.Net.Mail;
using axAssetControl.Entidades;
using Microsoft.Extensions.Options;

namespace axAssetControl.Negocio
{
    public class SendMail
    {
        private readonly EmailSettings _emailSettings;

        public SendMail(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(_emailSettings.From, _emailSettings.DisplayName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(new MailAddress(toEmail));

                using var smtp = new SmtpClient
                {
                    Host = _emailSettings.Host,
                    Port = _emailSettings.Port,
                    EnableSsl = _emailSettings.EnableSSL,
                    Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password)
                };

                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al enviar el correo electronico");
            }

        }
    }
}
