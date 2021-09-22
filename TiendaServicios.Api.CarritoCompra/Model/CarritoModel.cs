using System;
using System.Collections.Generic;

namespace TiendaServicios.Api.CarritoCompra.Model
{
    public class CarritoModel
    {
        public int CarritoId { get; set; }
        public DateTime? FechaCreacionSesion { get; set; }
        public List<CarritoDetalleModel> ListaProductos { get; set; }
    }
}
