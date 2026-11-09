using Microsoft.EntityFrameworkCore;
using Refresca.Api.Domain.Entities;

namespace Refresca.Api.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task InitializeAsync(RefrescaDbContext db)
    {
        if (await db.PontosRefresca.AnyAsync()) return;

        // Dados fictícios para demonstração acadêmica; não representam estabelecimentos reais.
        db.PontosRefresca.AddRange(
            new PontoRefresca("Ponto Refresca - Praça de exemplo", -23.5888m, -46.6340m, true, true, false, "TOT-001"),
            new PontoRefresca("Ponto Refresca - Comércio parceiro fictício", -23.5902m, -46.6381m, true, true, true, "TOT-002"),
            new PontoRefresca("Ponto Refresca - Área de apoio simulada", -23.5859m, -46.6322m, false, true, false, "TOT-003")
        );
        await db.SaveChangesAsync();
    }
}
