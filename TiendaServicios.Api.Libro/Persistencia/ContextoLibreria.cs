using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Models;

namespace TiendaServicios.Api.Libro.Persistencia
{
    public class ContextoLibreria : DbContext
    {
        public ContextoLibreria() { }

        public ContextoLibreria(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<LibreriaMaterial> LibreriaMateria { get; set; }
    }
}
