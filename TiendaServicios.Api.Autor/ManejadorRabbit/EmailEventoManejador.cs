using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Interface;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Modelo;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;

namespace TiendaServicios.Api.Autor.ManejadorRabbit
{
    public class EmailEventoManejador : IEventoManejador<EmailEventoQueue>
    {
        private readonly ILogger<EmailEventoManejador> _logger;
        private readonly ISendGridEnviar _sendGrid;
        private readonly IConfiguration _configuration;

        public EmailEventoManejador() { }
        public EmailEventoManejador(ILogger<EmailEventoManejador> logger, ISendGridEnviar sendGrid, IConfiguration configuration)
        {
            _logger = logger;
            _sendGrid = sendGrid;
            _configuration = configuration;
        }

        public async Task Handle(EmailEventoQueue @event)
        {
            _logger.LogInformation($"Este es el valor que consumo desde RabbitMQ: {@event.Titulo}");

            var mensaje = new SendGridData();
            mensaje.Contenido = @event.Contenido;
            mensaje.EmailDestinatario = @event.Destinatario;
            mensaje.NombreDestinatario = @event.Destinatario;
            mensaje.Titulo = @event.Titulo;
            mensaje.SendGridAPIKey = _configuration["SendGrid:ApiKey"];

            var result = await _sendGrid.EnviarEmail(mensaje);

            if(result.resultado)
            {
                await Task.CompletedTask;
                return;
            }
        }
    }
}
