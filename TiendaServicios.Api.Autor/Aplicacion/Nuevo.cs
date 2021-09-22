using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime? FechaNacimiento { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
                RuleFor(x => x.FechaNacimiento).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoAutor _context;
            public Manejador(ContextoAutor context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var autorLibro = new AutorLibro
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    FechaNacimiento = request.FechaNacimiento,
                    AutorLibroGuid = Guid.NewGuid().ToString()
                };

                await _context.AutorLibro.AddAsync(autorLibro);

                var resultado = await _context.SaveChangesAsync();

                if(resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el autor del libro");
            }
        }

    }
}
