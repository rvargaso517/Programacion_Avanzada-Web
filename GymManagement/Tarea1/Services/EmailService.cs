using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Tarea1.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void EnviarEnlaceRecuperacion(string destinatario, string nombreUsuario, string enlace)
        {
            var subject = "Recuperación de Contraseña - Gym Management";
            var body = $@"
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); border: 1px solid #ddd;'>
        <h2 style='color: #f36100; text-align: center;'>Gym Management</h2>
        <h3 style='border-bottom: 2px solid #f36100; padding-bottom: 10px; color: #333;'>Recuperación de Contraseña</h3>
        <p>Hola <strong>{nombreUsuario}</strong>,</p>
        <p>Has solicitado restablecer tu contraseña. Para continuar con el proceso, haz clic en el siguiente enlace:</p>
        <div style='text-align: center; margin: 30px 0;'>
            <a href='{enlace}' style='background-color: #f36100; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Restablecer Contraseña</a>
        </div>
        <p>Este enlace es de uso único y expirará en 60 minutos.</p>
        <p style='color: #777; font-size: 12px;'>Si no solicitaste este cambio, puedes ignorar este correo de forma segura.</p>
        <hr style='border: none; border-top: 1px solid #eee; margin-top: 30px;'>
        <p style='text-align: center; color: #777; font-size: 11px;'>
            ¡Gracias por ser parte de Gym Management!
        </p>
    </div>
</body>
</html>";

            EnviarCorreo(destinatario, subject, body);
        }

        public void EnviarRecordatorioPago(string destinatario, string nombreCliente, string planNombre, int diasRestantes)
        {
            var subject = "Recordatorio de Pago y Vencimiento de Membresía - Gym Management";
            var estadoMembresia = diasRestantes <= 0 ? "Vencida" : $"Próxima a vencer ({diasRestantes} días restantes)";
            var body = $@"
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); border: 1px solid #ddd;'>
        <h2 style='color: #f36100; text-align: center;'>Gym Management</h2>
        <h3 style='border-bottom: 2px solid #f36100; padding-bottom: 10px; color: #333;'>Aviso de Vencimiento de Membresía</h3>
        <p>Hola <strong>{nombreCliente}</strong>,</p>
        <p>Te recordamos que tu membresía asociada al plan <strong>{planNombre}</strong> está <strong>{estadoMembresia}</strong>.</p>
        <p>Te invitamos a acercarte a la recepción del gimnasio para renovar tu membresía y continuar entrenando con nosotros.</p>
        <hr style='border: none; border-top: 1px solid #eee; margin-top: 30px;'>
        <p style='text-align: center; color: #777; font-size: 11px;'>
            ¡Gracias por entrenar con nosotros!
        </p>
    </div>
</body>
</html>";

            EnviarCorreo(destinatario, subject, body);
        }

        private void EnviarCorreo(string destinatario, string subject, string body)
        {
            try
            {
                var smtpHost = _config["EmailSettings:SmtpHost"];
                var smtpPortVal = _config["EmailSettings:SmtpPort"];
                var senderEmail = _config["EmailSettings:SenderEmail"];
                var senderPassword = _config["EmailSettings:SenderPassword"];

                if (!string.IsNullOrEmpty(smtpHost) && int.TryParse(smtpPortVal, out int smtpPort)
                    && !string.IsNullOrEmpty(senderEmail) && !string.IsNullOrEmpty(senderPassword))
                {
                    using var mail = new MailMessage();
                    mail.From = new MailAddress(senderEmail, "Gym Management");
                    mail.To.Add(destinatario);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using var smtp = new SmtpClient(smtpHost, smtpPort);
                    smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
                else
                {
                    // Sin credenciales SMTP configuradas: el correo se guarda como archivo
                    // dentro de la carpeta de la aplicación, para poder revisarlo en pruebas.
                    var dir = Path.Combine(AppContext.BaseDirectory, "EmailsSimulated");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var nombreArchivo = $"Email_{DateTime.Now:yyyyMMdd_HHmmss}_{LimpiarNombre(destinatario)}.html";
                    var filePath = Path.Combine(dir, nombreArchivo);
                    File.WriteAllText(filePath, body);
                    Console.WriteLine($"[EMAIL MOCK] No hay credenciales SMTP configuradas. Correo guardado en: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
            }
        }

        /// <summary>Quita del correo del destinatario los caracteres que no valen en un nombre de archivo.</summary>
        private static string LimpiarNombre(string destinatario) =>
            string.Concat(destinatario.Split(Path.GetInvalidFileNameChars()));
    }
}
