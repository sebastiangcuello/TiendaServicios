using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Persistencia;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Titulo { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public Guid? AutorLibroId { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.AutorLibroId).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoLibreria _context;
            private readonly IRabbitEventBus _eventBus;
            public Manejador(ContextoLibreria context, IRabbitEventBus eventBus)
            {
                _context = context;
                _eventBus = eventBus;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libreriaMaterial = new LibreriaMaterial
                {
                    LibreriaMaterialId = Guid.NewGuid(),
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibroId = request.AutorLibroId
                };

                await _context.LibreriaMaterial.AddAsync(libreriaMaterial);

                var resultado = await _context.SaveChangesAsync();

                _eventBus.Publish(new EmailEventoQueue("sebastiangcuello@hotmail.com", request.Titulo, "Este contenido es un ejemplo"));

                if (resultado > 0)
                {
                    return Unit.Value;
                }     

                throw new Exception("No se pudo insertar la librería material");
            }
        }

    }
}
