﻿using System;

namespace TiendaServicios.Api.Gateway.LibroRemote
{
    public class LibroModeloRemote
    {
        public Guid? LibreriaMaterialId { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public Guid? AutorLibroId { get; set; }

        public AutorModeloRemote AutorModeloRemote { get; set; }
    }
}
