using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Model;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;

namespace TiendaServicios.Api.CarritoCompra.RemoteService
{
    public class LibrosService : ILibrosService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<LibrosService> _logger;

        public LibrosService(IHttpClientFactory httpClient, ILogger<LibrosService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<(bool resultado, LibroRemote Libro, string ErrorMessage)> GetLibro(Guid LibroId)
        {
            try
            {
                var client = _httpClient.CreateClient("Libros");
                var response = await client.GetAsync($"api/LibreriaMaterial/{LibroId}");

                if(response.IsSuccessStatusCode){  
                        var contenido = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true}; //Evitar que de problemas con mayusculas y minusculas del json
                        var resultado = JsonSerializer.Deserialize<LibroRemote>(contenido, options);

                    return (true, resultado, string.Empty);
                }

                return (false, null, $"Ha ocurrido un error al obtener el libro con id {LibroId} => {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                string mensajeError = $"Ha ocurrido un error al obtener el libro con id {LibroId} => {ex}";
                _logger.LogError(mensajeError);

                return (false, null, mensajeError);
            }
        }
    }
}
