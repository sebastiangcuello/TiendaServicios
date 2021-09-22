using AutoMapper;
using TiendaServicios.Api.Libro.Model;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion.Mapper
{
    public class MappingProfileLibro : Profile
    {
        public MappingProfileLibro()
        {
            CreateMap<LibreriaMaterial, LibreriaMaterialModel>();
        }
    }
}
