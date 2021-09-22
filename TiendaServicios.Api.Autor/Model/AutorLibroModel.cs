using System;

namespace TiendaServicios.Api.Autor.Model
{
    public class AutorLibroModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string AutorLibroGuid {get;set;}
    }
}
