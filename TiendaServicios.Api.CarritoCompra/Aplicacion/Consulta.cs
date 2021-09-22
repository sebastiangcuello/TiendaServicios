using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Model;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using System.Linq;
using System.Collections.Generic;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<CarritoModel>
        {
            public int CarritoSesionId { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.CarritoSesionId).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, CarritoModel>
        {
            private readonly ContextoCarrito _context;
            private readonly ILibrosService _librosService;
            public Manejador(ContextoCarrito context, ILibrosService librosService)
            {
                _context = context;
                _librosService = librosService;
            }

            public async Task<CarritoModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = await _context.CarritoSesion.FirstOrDefaultAsync(f=> f.CarritoSesionId == request.CarritoSesionId);
                var carritoSesionDetalle = await _context.CarritoSesionDetalle.Where(w => w.CarritoSesionId == request.CarritoSesionId).ToListAsync();

                var listaCarritoModel = new List<CarritoDetalleModel>();

                foreach(var libro in carritoSesionDetalle) {

                    var response = await _librosService.GetLibro(new Guid(libro.ProductoSeleccionado));
                    if(response.resultado)
                    {
                        var objetoLibro = response.Libro;
                        var carritoDetalle = new CarritoDetalleModel
                        {
                            TituloLibro = objetoLibro.Titulo,
                            FechaPublicacion = objetoLibro.FechaPublicacion,
                            LibroId = objetoLibro.LibreriaMaterialId
                        };

                        listaCarritoModel.Add(carritoDetalle);
                    }
                }

                return new CarritoModel
                {
                    CarritoId = request.CarritoSesionId,
                    FechaCreacionSesion = DateTime.Now,
                    ListaProductos = listaCarritoModel
                };
            }
        }
    }
}
