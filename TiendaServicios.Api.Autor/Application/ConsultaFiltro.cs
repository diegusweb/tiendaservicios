using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Models;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Application
{
    public class ConsultaFiltro
    {
        public class AutorUnico : IRequest<AutorDto> { 
            public string AutorGuid { get; set; }
        }

        public class Manejador : IRequestHandler<AutorUnico, AutorDto>
        {
            private readonly ContextoAutor _contexto;

            private readonly IMapper _mapper;

            public Manejador(ContextoAutor contexto, IMapper mapper) {
                _contexto = contexto;
                _mapper = mapper;
            }
            public async Task<AutorDto> Handle(AutorUnico request, CancellationToken cancellationToken)
            {
                var autor = await _contexto.AutorLibros.Where(x => x.AutorLibroGuid == request.AutorGuid).FirstOrDefaultAsync();

                if (autor == null) {
                    throw new Exception("NO se encuentra autor");
                }

                var autorDto = _mapper.Map<AutorLibro, AutorDto>(autor);

                return autorDto;
            }
        }
    }
}
