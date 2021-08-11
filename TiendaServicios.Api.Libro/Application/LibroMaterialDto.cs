using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Libro.Application
{
    public class LibroMaterialDto
    {
        public Guid? LibreriaMaterialId { get; set; }

        public string Titulo { get; set; }

        public DateTime? FechaPublicaion { get; set; }

        public Guid? AutorLibro { get; set; }
    }
}
