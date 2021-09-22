using System;
using System.Collections.Generic;

namespace TiendaServicios.Api.CarritoCompra.Persistencia
{
    public class CarritoSesion
    {
        public int CarritoSesionId { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public ICollection<CarritoSesionDetalle> ListaDetalle { get; set; }
    }
}
