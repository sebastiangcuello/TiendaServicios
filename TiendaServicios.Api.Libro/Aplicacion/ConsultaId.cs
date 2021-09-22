using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Model;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class ConsultaId
    {
        public class Ejecuta : IRequest<LibreriaMaterialModel>
        {
            public string LibreriaMaterialGuid { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, LibreriaMaterialModel>
        {
            private readonly ContextoLibreria _context;
            private readonly IMapper _mapper;
            public Manejador(ContextoLibreria context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<LibreriaMaterialModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                var libreria = await _context.LibreriaMaterial.FirstOrDefaultAsync(f => f.LibreriaMaterialId.ToString() == request.LibreriaMaterialGuid);

                if (libreria == null)
                {
                    throw new Exception("El libro no existe");
                }

                return _mapper.Map<LibreriaMaterialModel>(libreria);
            }
        }
    }
}
