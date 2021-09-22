using System;

namespace TiendaServicios.Api.CarritoCompra.Model
{
    public class CarritoDetalleModel
    {
        public Guid? LibroId { get; set; }
        public string TituloLibro { get; set; }
        public string AutorLibro { get; set; }
        public DateTime? FechaPublicacion { get; set; }
    }
}
