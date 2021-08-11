using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Models;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Application
{
    public class Consultacs
    {
        public class Ejecuta : IRequest<List<LibroMaterialDto>>
        { 
        
        }

        public class Manejador : IRequestHandler<Ejecuta, List<LibroMaterialDto>>
        {
            private readonly ContextoLibreria _contexto;

            private readonly IMapper _mapper;

            public Manejador(ContextoLibreria contexto, IMapper mapper) 
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<List<LibroMaterialDto>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libros = await _contexto.LibreriaMateria.ToListAsync();
               var librosDto = _mapper.Map<List<LibreriaMaterial>, List<LibroMaterialDto>>(libros);

                return librosDto;
            }
        }
    }
}
