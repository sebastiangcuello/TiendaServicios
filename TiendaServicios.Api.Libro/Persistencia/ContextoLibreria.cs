using Microsoft.EntityFrameworkCore;

namespace TiendaServicios.Api.Libro.Persistencia
{
    public class ContextoLibreria : DbContext
    {
        public ContextoLibreria()
        {
        }

        public ContextoLibreria(DbContextOptions options) : base(options) { }

        //virtual para que deje sobrescribirla (sino no funcionarían los tests)
        public virtual DbSet<LibreriaMaterial> LibreriaMaterial { get; set; }
    }
}
