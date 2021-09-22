using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Aplicacion;
using TiendaServicios.Api.CarritoCompra.Model;

namespace TiendaServicios.Api.CarritoCompra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CarritoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Nuevo.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarritoModel>> GetCarrito(int id)
        {
            return await _mediator.Send(new Consulta.Ejecuta { CarritoSesionId = id });
        }
    }
}
