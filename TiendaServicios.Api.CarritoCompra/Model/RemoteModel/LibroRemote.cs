using System;

namespace TiendaServicios.Api.CarritoCompra.Model
{
    public class LibroRemote
    {
        public Guid? LibreriaMaterialId { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public Guid? AutorLibroId { get; set; }
    }
}
