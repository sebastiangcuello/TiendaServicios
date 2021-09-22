using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Model;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class ConsultaId
    {
        public class Ejecuta : IRequest<AutorLibroModel>
        {
            public string AutorGuid { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, AutorLibroModel>
        {
            private readonly ContextoAutor _context;
            private readonly IMapper _mapper;
            public Manejador(ContextoAutor context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<AutorLibroModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                var autor = await _context.AutorLibro.FirstOrDefaultAsync(f=> f.AutorLibroGuid == request.AutorGuid);

                if(autor == null)
                {
                    throw new Exception("El autor no existe");
                }

                return _mapper.Map<AutorLibroModel>(autor);
            }
        }
    }
}
