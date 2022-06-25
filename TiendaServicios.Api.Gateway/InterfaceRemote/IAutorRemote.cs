using System;
using System.Threading.Tasks;
using TiendaServicios.Api.Gateway.LibroRemote;

namespace TiendaServicios.Api.Gateway.InterfaceRemote
{
    public interface IAutorRemote
    {
        Task<(bool Resultado, AutorModeloRemote AutorRemote, string ErrorMessage)> GetAutor(Guid AutorId);
    }
}
