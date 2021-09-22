using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Model;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<List<AutorLibroModel>>
        {
        }

        public class Manejador : IRequestHandler<Ejecuta, List<AutorLibroModel>>
        {
            private readonly ContextoAutor _context;
            private readonly IMapper _mapper;
            public Manejador(ContextoAutor context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<AutorLibroModel>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                var autores = await _context.AutorLibro.ToListAsync();

                return _mapper.Map<List<AutorLibroModel>>(autores);
            }
        }
    }
}
