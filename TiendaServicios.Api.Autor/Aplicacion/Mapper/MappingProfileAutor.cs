using AutoMapper;
using TiendaServicios.Api.Autor.Persistencia;
using TiendaServicios.Api.Autor.Model;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class MappingProfileAutor : Profile
    {
        public MappingProfileAutor()
        {
            CreateMap<AutorLibro, AutorLibroModel>();
            CreateMap<GradoAcademico, GradoAcademicoModel>();
        }
    }
}
