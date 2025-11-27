using Microsoft.EntityFrameworkCore;
using Obligatorio.LogicaNegocio.Entidades;



namespace Obligatorio.AccesoDatos
{
	public class OblContext : DbContext
	{
		// Constructor requerido para DI (AddDbContext en Program.cs)
		public OblContext(DbContextOptions<OblContext> options) : base(options)
		{
		}

		// DbSets
		public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<Pago> Pagos { get; set; }
		public DbSet<Equipo> Equipos { get; set; }
		public DbSet<Recurrente> Recurrentes { get; set; }
		public DbSet<Unico> Unicos { get; set; }
		public DbSet<TipoGasto> TipoGastos { get; set; }
		public DbSet<Auditoria> Auditorias { get; set; }

		// Mapeos y configuraciones del modelo
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Herencia TPH para Pago (Unico y Recurrente en la misma tabla con discriminador)
			modelBuilder.Entity<Pago>()
				.HasDiscriminator<string>("PagoTipo")
				.HasValue<Unico>("Unico")
				.HasValue<Recurrente>("Recurrente");


			modelBuilder.Entity<Usuario>()
	       .HasMany(u => u.Pagos)
	        .WithOne(p => p.Usuario)
	       .HasForeignKey(p => p.UsuarioId)
	       .OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Auditoria>(b =>
			{
				b.ToTable("Auditorias");
				b.HasKey(a => a.Id);
				b.Property(a => a.Accion).HasMaxLength(50).IsRequired();
				b.Property(a => a.Fecha).IsRequired();
				b.HasOne(a => a.Usuario).WithMany().HasForeignKey(a => a.UsuarioId).OnDelete(DeleteBehavior.Restrict);
			}

				);
			}
		}



}
