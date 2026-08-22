using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace GymManagement_API.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void EnviarRecibo(string destinatario, string nombreCliente, decimal monto, string metodoPago, string? planNombre)
        {
            var subject = "Recibo de Pago - Gym Management";
            var body = $@"
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); border: 1px solid #ddd;'>
        <h2 style='color: #f36100; text-align: center;'>Gym Management</h2>
        <h3 style='border-bottom: 2px solid #f36100; padding-bottom: 10px; color: #333;'>Confirmación de Pago (Recibo)</h3>
        <p>Hola <strong>{nombreCliente}</strong>,</p>
        <p>Confirmamos que hemos recibido tu pago correspondiente al gimnasio. A continuación los detalles de la transacción:</p>
        <table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>
            <tr style='background-color: #f8f8f8;'>
                <td style='padding: 10px; border: 1px solid #ddd; font-weight: bold;'>Monto Pagado:</td>
                <td style='padding: 10px; border: 1px solid #ddd; color: green; font-weight: bold;'>₡{monto:N2}</td>
            </tr>
            <tr>
                <td style='padding: 10px; border: 1px solid #ddd; font-weight: bold;'>Concepto/Plan:</td>
                <td style='padding: 10px; border: 1px solid #ddd;'>{(string.IsNullOrEmpty(planNombre) ? "Pago General / Membresía" : planNombre)}</td>
            </tr>
            <tr style='background-color: #f8f8f8;'>
                <td style='padding: 10px; border: 1px solid #ddd; font-weight: bold;'>Método de Pago:</td>
                <td style='padding: 10px; border: 1px solid #ddd;'>{metodoPago}</td>
            </tr>
            <tr>
                <td style='padding: 10px; border: 1px solid #ddd; font-weight: bold;'>Fecha y Hora:</td>
                <td style='padding: 10px; border: 1px solid #ddd;'>{DateTime.Now:dd/MM/yyyy hh:mm tt}</td>
            </tr>
        </table>
        <p style='text-align: center; color: #777; font-size: 12px; margin-top: 30px;'>
            ¡Gracias por entrenar con nosotros! Si tienes alguna duda, responde a este correo.
        </p>
    </div>
</body>
</html>";

            try
            {
                // Leer configuración SMTP si existe
                var smtpHost = _config["EmailSettings:SmtpHost"];
                var smtpPortVal = _config["EmailSettings:SmtpPort"];
                var senderEmail = _config["EmailSettings:SenderEmail"];
                var senderPassword = _config["EmailSettings:SenderPassword"];

                if (!string.IsNullOrEmpty(smtpHost) && int.TryParse(smtpPortVal, out int smtpPort) && !string.IsNullOrEmpty(senderEmail))
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
                    // Guardar localmente como simulacro de correo (Mock)
                    var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReceiptsSimulated");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var filePath = Path.Combine(dir, $"Recibo_{DateTime.Now:yyyyMMdd_HHmmss}_{destinatario}.html");
                    File.WriteAllText(filePath, body);
                    Console.WriteLine($"[EMAIL MOCK] Recibo simulado guardado en: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el recibo por correo: {ex.Message}");
            }
        }

        public void EnviarPasswordTemporal(string destinatario, string nombreCliente, string passwordTemporal)
        {
            var subject = "Tu cuenta de Gym Management ha sido creada";
            var body = $@"
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); border: 1px solid #ddd;'>
        <h2 style='color: #f36100; text-align: center;'>Gym Management</h2>
        <h3 style='border-bottom: 2px solid #f36100; padding-bottom: 10px; color: #333;'>¡Bienvenido a Gym Management!</h3>
        <p>Hola <strong>{nombreCliente}</strong>,</p>
        <p>Tu cuenta ha sido creada por un administrador. A continuación, te proporcionamos tus credenciales temporales para acceder al sistema:</p>
        <div style='background-color: #f8f8f8; padding: 15px; border-radius: 5px; border: 1px solid #eee; margin: 20px 0;'>
            <p style='margin: 5px 0;'><strong>Usuario (Correo):</strong> {destinatario}</p>
            <p style='margin: 5px 0;'><strong>Contraseña Temporal:</strong> <span style='font-family: monospace; font-size: 16px; color: #f36100; font-weight: bold;'>{passwordTemporal}</span></p>
        </div>
        <p>Te recomendamos cambiar tu contraseña al iniciar sesión por primera vez desde tu perfil.</p>
        <p style='text-align: center; color: #777; font-size: 12px; margin-top: 30px;'>
            ¡Gracias por unirte a nosotros! Si tienes alguna duda, responde a este correo.
        </p>
    </div>
</body>
</html>";

            try
            {
                var smtpHost = _config["EmailSettings:SmtpHost"];
                var smtpPortVal = _config["EmailSettings:SmtpPort"];
                var senderEmail = _config["EmailSettings:SenderEmail"];
                var senderPassword = _config["EmailSettings:SenderPassword"];

                if (!string.IsNullOrEmpty(smtpHost) && int.TryParse(smtpPortVal, out int smtpPort) && !string.IsNullOrEmpty(senderEmail))
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
                    var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReceiptsSimulated");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var filePath = Path.Combine(dir, $"PasswordTemporal_{DateTime.Now:yyyyMMdd_HHmmss}_{destinatario}.html");
                    File.WriteAllText(filePath, body);
                    Console.WriteLine($"[EMAIL MOCK] Contraseña temporal simulada guardada en: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo con contraseña temporal: {ex.Message}");
            }
        }
    }
}
