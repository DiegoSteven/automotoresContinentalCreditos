using Microsoft.EntityFrameworkCore;
using RespuestaCredito.Models; // Importante para ver tus modelos

namespace RespuestaCredito.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SolicitudCredito> Solicitudes { get; set; }
        public DbSet<RespuestaCreditoFinanciera> RespuestasFinanciera { get; set; }
        public DbSet<NotificacionAsesor> Notificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SolicitudCredito>()
                .HasIndex(s => s.NumeroSolicitud).IsUnique();
        }
    }
}