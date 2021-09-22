using AutoMapper;
using GenFu;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Model;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Tests
{
    public class LibrosServiceTest
    {
        private IEnumerable<LibreriaMaterial> ObtenerDataPrueba()
        {
            //Creo la data de prueba
            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMaterialId, () => { return Guid.NewGuid(); });

            //Devuelvo el listado con la cantidad que quiero
            var lista = A.ListOf<LibreriaMaterial>(30);

            //Al primer registro le pongo un guid vacio ya que me va a servir para el GetLibroPorId()
            lista[0].LibreriaMaterialId = Guid.Empty;

            return lista;
        }

        private Mock<ContextoLibreria> CrearContexto()
        {
            var dataPrueba = ObtenerDataPrueba().AsQueryable();

            var dbSet = new Mock<DbSet<LibreriaMaterial>>();

            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

            dbSet.As<IAsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new CancellationToken()))
                .Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

            //Posibilita los filtros por libreria material
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<LibreriaMaterial>(dataPrueba.Provider));

            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(s => s.LibreriaMaterial).Returns(dbSet.Object);

            return contexto;
        }

        [Fact]
        public async void GetLibroPorId()
        {
            var mockContexto = CrearContexto();

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();

            var manejador = new ConsultaId.Manejador(mockContexto.Object, mapper);
            var request = new ConsultaId.Ejecuta {LibreriaMaterialGuid = Guid.Empty.ToString() };

            var libros = await manejador.Handle(request, new CancellationToken());

            Assert.NotNull(libros);
            Assert.True(libros.LibreriaMaterialId == Guid.Empty);
        }

        [Fact]
        public async void GetLibros()
        {
            //Buscar metodo de microservicios que consulta los libros
            //1. Emular a la instancia de entity framework core - ContextoLibreria
            //Para emular acciones y eventos de un objeto en un ambiente de unit test utilizamos
            //objetos de tipo mock

            var mockContexto = CrearContexto();

            //2. Emulamos el Mapper

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();

            //3. Instanciamos a la clase Manejador y le pasamos los parametros de los 2 mocks

            var manejador = new Consulta.Manejador(mockContexto.Object, mapper);
            var request = new Consulta.Ejecuta();

            var libros = await manejador.Handle(request, new CancellationToken());

            Assert.True(libros.Any());
         
        }

        [Fact]
        public async void GuardarLibro()
        {
            //Creo la base de datos en memoria con la libreria Microsoft.EntityFrameworkCore.InMemory
            var options = new DbContextOptionsBuilder<ContextoLibreria>()
                    .UseInMemoryDatabase(databaseName: "BaseDatosLibro")
                    .Options;

            var context = new ContextoLibreria(options);

            var request = new Nuevo.Ejecuta
            {
                AutorLibroId = Guid.Empty,
                FechaPublicacion = DateTime.Now,
                Titulo = "Libro de Microservicios"
            };

            var manejador = new Nuevo.Manejador(context);

            var resultado = await manejador.Handle(request, new CancellationToken());

            Assert.True(resultado == Unit.Value);
        }
    }
}
