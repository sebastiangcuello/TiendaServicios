using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Interface;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Modelo;

namespace TiendaServicios.Mensajeria.Email.SendGridLibreria.Implement
{
    public class SendGridEnviar : ISendGridEnviar
    {
        public async Task<(bool resultado, string errorMensaje)> EnviarEmail(SendGridData data)
        {
            try
            {
                var sendGridCliente = new SendGridClient(data.SendGridAPIKey);
                var destinatario = new EmailAddress(data.EmailDestinatario, data.NombreDestinatario);
                var sender = new EmailAddress("sebastian@hotmail.com", "Sebastián");
                var titulo = data.Titulo;
                var contenido = data.Contenido;
                var mensaje = MailHelper.CreateSingleEmail(sender, destinatario, titulo, contenido, contenido);

                var response = await sendGridCliente.SendEmailAsync(mensaje);

                if(!response.IsSuccessStatusCode)
                {
                    return (false, $"Error Code: {response.StatusCode}");
                }
            }
            catch (Exception)
            {
                return (false, "Error enviando email");
                throw;
            }

            return (true, string.Empty);
        }
    }
}
