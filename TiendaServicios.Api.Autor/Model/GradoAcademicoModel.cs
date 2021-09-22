using System;

namespace TiendaServicios.Api.Autor.Model
{
    public class GradoAcademicoModel
    {
        public string Nombre { get; set; }
        public string CentroAcademico { get; set; }
        public DateTime? FechaGrado { get; set; }
        public int AutorLibroId { get; set; }
        public string GradoAcademicoGuid { get; set; }
    }
}
