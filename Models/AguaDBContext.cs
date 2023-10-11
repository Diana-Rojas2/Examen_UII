using Microsoft.EntityFrameworkCore;

namespace Examen_UII.Models
{
    public class AguaDBContext : DbContext
    {
        public AguaDBContext()
        {
        }

        public AguaDBContext(DbContextOptions<AguaDBContext> options) : base(options)
        {
        }

        public DbSet<Roles> Roles { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Municipios> Municipios { get; set; }
        public DbSet<Zonas> Zonas { get; set; }
        public DbSet<Dispositivos> Dispositivos { get; set; }
        public DbSet<RegistrosConsumo> RegistrosConsumo { get; set; }
        public DbSet<Estados> Estados { get; set; }
    }
}
