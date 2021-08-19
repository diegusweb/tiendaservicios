using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiendaServicios.Api.Libro.Application;
using TiendaServicios.Api.Libro.Models;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libros.Tests
{
    public class LibrosServiceTest
    {
        private IEnumerable<LibreriaMaterial> ObtenerDataPrueba()
        {
            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMaterialId, () => { return Guid.NewGuid(); });

            var lista = A.ListOf<LibreriaMaterial>(30);
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

            dbSet.As<IAsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
                .Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

            //se necesita esto para poder hacer filtro por libro id
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider)
                .Returns(new AsyncQueryProvider<LibreriaMaterial>(dataPrueba.Provider));


            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x => x.LibreriaMateria).Returns(dbSet.Object);
            return contexto;
        }


        [Fact]
        public async void GetLibroById()
        {
            var mockContexto = CrearContexto();

            var mapConfig = new MapperConfiguration(cfg =>
           {
               cfg.AddProfile(new MappingTest());
           });

            var mapper = mapConfig.CreateMapper();

            // 

            var request = new ConsultaFiltro.LibroUnico();
            request.LibroId = Guid.Empty;

            //instanciar al objeto manejador
            var manejador = new ConsultaFiltro.Manejador(mockContexto.Object, mapper);

            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.NotNull(libro);
            Assert.True(libro.LibreriaMaterialId == Guid.Empty);
        }


        [Fact]
        public async void GetLibros()
        {
            //System.Diagnostics.Debugger.Launch();
            //1. emular  ala instancia de entity framework core
            // para emular las acciones y eventos de un objeto en unit test utilizamos objetos de tipo mock

            //var mockContexto = new Mock<ContextoLibreria>();
            var mockContexto = CrearContexto();

            // 2 emular al mapping Imapper
            //var mockMapper = new Mock<IMapper>();
            var mapConfig = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile(new MappingTest());
            });

            var mockMapper = mapConfig.CreateMapper();

            //3 instancia a la clase manejador y pasar como parametros los mocks que he creado

            Consultacs.Manejador manejador = new Consultacs.Manejador(mockContexto.Object, mockMapper);

            //4 intancia de ejecuta
            Consultacs.Ejecuta request = new Consultacs.Ejecuta();

            var lista = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(lista.Any());


        }

        [Fact]
        public async void GuardarLibro()
        {
            var options = new DbContextOptionsBuilder<ContextoLibreria>()
                .UseInMemoryDatabase(databaseName: "BaseDatosLibro")
                .Options;

            var contexto = new ContextoLibreria(options);

            var request = new Nuevo.Ejecuta();
            request.Titulo = "Libro de Microservicios";
            request.AutorLibro = Guid.Empty;
            request.FechaPublicacion = DateTime.Now;

            var manejador = new Nuevo.Manejador(contexto);

            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(libro != null);

        }
    }
}
