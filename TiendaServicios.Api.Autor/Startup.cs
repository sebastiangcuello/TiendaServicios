using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.ManejadorRabbit;
using TiendaServicios.Api.Autor.Persistencia;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Implement;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Interface;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;
using TiendaServicios.RabbitMQ.Bus.Implement;

namespace TiendaServicios.Api.Autor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEventoManejador<EmailEventoQueue>, EmailEventoManejador>();

            services.AddSingleton<IRabbitEventBus, RabbitEventBus>(sp => {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

                return new RabbitEventBus(sp.GetService<IMediator>(), scopeFactory);
            });

            services.AddTransient<EmailEventoManejador>();

            services.AddSingleton<ISendGridEnviar, SendGridEnviar>();

            services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            services.AddDbContext<ContextoAutor>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("ConexionDatabase"));
            });

            services.AddMediatR(typeof(Nuevo.Manejador).Assembly);
            services.AddAutoMapper(typeof(MappingProfileAutor));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var eventBus = app.ApplicationServices.GetRequiredService<IRabbitEventBus>();
            eventBus.Subscribe<EmailEventoQueue, EmailEventoManejador>();
        }
    }
}
