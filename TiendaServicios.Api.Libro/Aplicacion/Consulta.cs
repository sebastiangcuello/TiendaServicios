using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Model;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<List<LibreriaMaterialModel>>
        {
        }

        public class Manejador : IRequestHandler<Ejecuta, List<LibreriaMaterialModel>>
        {
            private readonly ContextoLibreria _context;
            private readonly IMapper _mapper;
            public Manejador(ContextoLibreria context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<LibreriaMaterialModel>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                var autores = await _context.LibreriaMaterial.ToListAsync();

                return _mapper.Map<List<LibreriaMaterialModel>>(autores);
            }
        }
    }
}
