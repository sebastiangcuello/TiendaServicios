﻿using System;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Model;

namespace TiendaServicios.Api.CarritoCompra.RemoteInterface
{
    public interface ILibrosService
    {
        Task<(bool resultado, LibroRemote Libro, string ErrorMessage)>GetLibro(Guid LibroId);
    }
}
