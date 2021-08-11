using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Models;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Application
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        { 
            public string Titulo { get; set; }

            public DateTime? FechaPublicacion { get; set; }

            public Guid? AutorLibro { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion() {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.AutorLibro).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoLibreria _contexto;

            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libro = new LibreriaMaterial
                {
                    Titulo = request.Titulo,
                    AutorLibro = request.AutorLibro,
                    FechaPublicaion = request.FechaPublicacion
                };

                _contexto.LibreriaMateria.Add(libro);
                var value = await _contexto.SaveChangesAsync();

                if (value > 0)
                    return Unit.Value;

                throw new Exception("No se pudo insertar");
            }
        }
    }
}
