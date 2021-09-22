using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public DateTime FechaCreacionSesion { get; set; }
            public List<string> ListaProducto { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.FechaCreacionSesion).NotEmpty();
                RuleFor(x => x.ListaProducto).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoCarrito _context;
            public Manejador(ContextoCarrito context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = new CarritoSesion
                {
                    FechaCreacion = request.FechaCreacionSesion
                };

                await _context.CarritoSesion.AddAsync(carritoSesion);

                var resultado = await _context.SaveChangesAsync();

                if (resultado == 0)
                {
                    throw new Exception("No se pudo insertar el carrito sesion");
                }

                int id = carritoSesion.CarritoSesionId;

                foreach(var prod in request.ListaProducto)
                {
                    var carritoSesionDetalle = new CarritoSesionDetalle
                    {
                        CarritoSesionId = id,
                        FechaCreacion = request.FechaCreacionSesion,
                        ProductoSeleccionado = prod
                    };

                    _context.CarritoSesionDetalle.Add(carritoSesionDetalle);
                }

                resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el detalle de carrito de compras");
            }
        }

    }
}
