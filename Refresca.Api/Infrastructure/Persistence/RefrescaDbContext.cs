using Microsoft.EntityFrameworkCore;
using Refresca.Api.Domain.Entities;

namespace Refresca.Api.Infrastructure.Persistence;

public sealed class RefrescaDbContext(DbContextOptions<RefrescaDbContext> options)
    : DbContext(options)
{
    public DbSet<Leitura> Leituras => Set<Leitura>();
    public DbSet<PontoRefresca> PontosRefresca => Set<PontoRefresca>();
    public DbSet<PausaHidratacao> PausasHidratacao => Set<PausaHidratacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Leitura>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.TotemId).HasMaxLength(50).IsRequired();
            e.Property(x => x.Nivel).HasMaxLength(10).IsRequired();
            e.HasIndex(x => new { x.TotemId, x.MedidoEm });
        });

        modelBuilder.Entity<PontoRefresca>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).HasMaxLength(120).IsRequired();
            e.Property(x => x.TotemId).HasMaxLength(50).IsRequired();
            e.HasMany(x => x.Leituras).WithOne().HasForeignKey(x => x.TotemId)
                .HasPrincipalKey(x => x.TotemId);
        });

        modelBuilder.Entity<PausaHidratacao>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.TrabalhadorId).HasMaxLength(80).IsRequired();
            e.HasOne(x => x.PontoRefresca).WithMany()
                .HasForeignKey(x => x.PontoRefrescaId).OnDelete(DeleteBehavior.SetNull);
        });
    }
}
