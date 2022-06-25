﻿using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Gateway.InterfaceRemote;
using TiendaServicios.Api.Gateway.LibroRemote;

namespace TiendaServicios.Api.Gateway.MessageHandler
{
    public class LibroHandler : DelegatingHandler
    {
        private readonly ILogger<LibroHandler> _logger;
        private readonly IAutorRemote _autorRemote;
        public LibroHandler(ILogger<LibroHandler> logger, IAutorRemote autorRemote)
        {
            _logger = logger;
            _autorRemote = autorRemote;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tiempo = Stopwatch.StartNew();
            _logger.LogInformation($"Inicia el request: ");

            var response = await base.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
                var resultado = JsonSerializer.Deserialize<LibroModeloRemote>(contenido, options);

                var responseAutor = await _autorRemote.GetAutor(resultado.AutorLibroId ?? Guid.Empty);
                if (responseAutor.Resultado)
                {
                    var objetoAutor = responseAutor.AutorRemote;
                    resultado.AutorModeloRemote = objetoAutor;

                    var resultadoJson = JsonSerializer.Serialize(resultado, options);

                    response.Content = new StringContent(resultadoJson, System.Text.Encoding.UTF8, "application/json");
                }
            }

            _logger.LogInformation($"Tiempo de proceso: {tiempo.ElapsedMilliseconds}");

            return response;
        }
    }
}
