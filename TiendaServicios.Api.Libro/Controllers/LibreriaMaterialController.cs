using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Model;

namespace TiendaServicios.Api.Libro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibreriaMaterialController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LibreriaMaterialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Nuevo.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet]
        public async Task<ActionResult<List<LibreriaMaterialModel>>> GetLibrerias()
        {
            return await _mediator.Send(new Consulta.Ejecuta());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LibreriaMaterialModel>> GetLibreria(string id){
            return await _mediator.Send(new ConsultaId.Ejecuta { LibreriaMaterialGuid = id });
        }

    }
}
