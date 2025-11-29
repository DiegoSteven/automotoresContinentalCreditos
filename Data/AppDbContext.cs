using Microsoft.EntityFrameworkCore;
using RespuestaCredito.Models;

namespace RespuestaCredito.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SolicitudCredito> Solicitudes { get; set; }
        public DbSet<RespuestaCreditoFinanciera> RespuestasFinanciera { get; set; }
        public DbSet<NotificacionAsesor> Notificaciones { get; set; }
        public DbSet<Financiera> Financieras { get; set; }
        public DbSet<Asesor> Asesores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SolicitudCredito>()
                .HasIndex(s => s.NumeroSolicitud).IsUnique();

            modelBuilder.Entity<Asesor>()
                .HasIndex(a => a.Email).IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Username).IsUnique();
        }
    }
}