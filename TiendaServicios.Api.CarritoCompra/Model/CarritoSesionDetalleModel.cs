using System;

namespace TiendaServicios.Api.CarritoCompra.Model
{
    public class CarritoSesionDetalleModel
    {
        public int CarritoSesionDetalleId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string ProductoSeleccionado { get; set; }
    }
}
