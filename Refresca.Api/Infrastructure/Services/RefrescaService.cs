using Microsoft.EntityFrameworkCore;
using Refresca.Api.Application.DTOs;
using Refresca.Api.Application.Interfaces;
using Refresca.Api.Domain.Entities;
using Refresca.Api.Infrastructure.Persistence;

namespace Refresca.Api.Infrastructure.Services;

public sealed class RefrescaService(
    RefrescaDbContext db,
    IRepository<Leitura> leituras,
    IRepository<PausaHidratacao> pausas,
    IUnitOfWork unitOfWork) : IRefrescaService
{
    public async Task<LeituraResponseDto> RegistrarLeituraAsync(RegistrarLeituraDto dto, CancellationToken ct)
    {
        var pontoExiste = await db.PontosRefresca.AnyAsync(p => p.TotemId == dto.TotemId, ct);
        if (!pontoExiste) throw new KeyNotFoundException($"Totem '{dto.TotemId}' não está cadastrado.");

        var nivelCalculado = Leitura.ClassificarNivel(dto.IndiceCalor);
        if (!string.Equals(dto.Nivel, nivelCalculado, StringComparison.Ordinal))
            throw new ArgumentException($"Nível informado incompatível. Para índice {dto.IndiceCalor}, o nível esperado é '{nivelCalculado}'.");

        var leitura = new Leitura(dto.TotemId, dto.Temperatura, dto.Umidade, dto.IndiceCalor, dto.MedidoEm);
        await leituras.AddAsync(leitura, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return ToDto(leitura);
    }

    public async Task<IReadOnlyList<PontoResponseDto>> ListarPontosAsync(CancellationToken ct)
    {
        var pontos = await db.PontosRefresca.AsNoTracking()
            .Select(p => new
            {
                Ponto = p,
                Ultima = db.Leituras.Where(l => l.TotemId == p.TotemId)
                    .OrderByDescending(l => l.MedidoEm)
                    .Select(l => new UltimaLeituraDto(l.IndiceCalor, l.Nivel, l.MedidoEm))
                    .FirstOrDefault()
            }).ToListAsync(ct);

        return pontos.Select(x => ToPontoDto(x.Ponto, x.Ultima)).ToList();
    }

    public async Task<PontoResponseDto?> ObterPontoAsync(int id, CancellationToken ct)
    {
        var ponto = await db.PontosRefresca.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
        if (ponto is null) return null;

        var ultima = await db.Leituras.AsNoTracking()
            .Where(l => l.TotemId == ponto.TotemId)
            .OrderByDescending(l => l.MedidoEm)
            .Select(l => new UltimaLeituraDto(l.IndiceCalor, l.Nivel, l.MedidoEm))
            .FirstOrDefaultAsync(ct);

        return ToPontoDto(ponto, ultima);
    }

    public async Task<PausaResponseDto> RegistrarPausaAsync(RegistrarPausaDto dto, CancellationToken ct)
    {
        if (dto.PontoRefrescaId.HasValue &&
            !await db.PontosRefresca.AnyAsync(p => p.Id == dto.PontoRefrescaId.Value, ct))
            throw new KeyNotFoundException("Ponto Refresca não encontrado.");

        var pausa = new PausaHidratacao(dto.TrabalhadorId, dto.PontoRefrescaId,
            dto.RegistradaEm, dto.DuracaoMinutos);
        await pausas.AddAsync(pausa, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return new PausaResponseDto(pausa.Id, pausa.TrabalhadorId, pausa.PontoRefrescaId,
            pausa.RegistradaEm, pausa.DuracaoMinutos);
    }

    public async Task<IReadOnlyList<PausaResponseDto>> ListarPausasAsync(string trabalhadorId, CancellationToken ct)
    {
        return await db.PausasHidratacao.AsNoTracking()
            .Where(p => p.TrabalhadorId == trabalhadorId)
            .OrderByDescending(p => p.RegistradaEm)
            .Select(p => new PausaResponseDto(p.Id, p.TrabalhadorId, p.PontoRefrescaId,
                p.RegistradaEm, p.DuracaoMinutos))
            .ToListAsync(ct);
    }

    public async Task<AlertaHojeDto> ObterAlertaHojeAsync(CancellationToken ct)
    {
        var ultima = await db.Leituras.AsNoTracking()
            .OrderByDescending(l => l.MedidoEm).FirstOrDefaultAsync(ct);

        if (ultima is null)
            return new AlertaHojeDto("normal", null,
                "Ainda não há leituras cadastradas para gerar o alerta de hoje.", null,
                "Sem leituras registradas");

        var mensagem = ultima.Nivel switch
        {
            "perigo" => "Perigo: procure sombra, hidrate-se e faça uma pausa assim que possível.",
            "atencao" => "Atenção: aumente a hidratação e programe pausas em locais sombreados.",
            _ => "Condição dentro da faixa normal de acordo com a última leitura registrada."
        };

        return new AlertaHojeDto(ultima.Nivel, ultima.IndiceCalor, mensagem,
            ultima.MedidoEm, "Última leitura registrada pelos totens");
    }

    private static LeituraResponseDto ToDto(Leitura l) =>
        new(l.Id, l.TotemId, l.Temperatura, l.Umidade, l.IndiceCalor, l.Nivel, l.MedidoEm);

    private static PontoResponseDto ToPontoDto(PontoRefresca p, UltimaLeituraDto? ultima) =>
        new(p.Id, p.Nome, p.Latitude, p.Longitude, p.TemAgua, p.TemSombra, p.TemBanheiro, ultima);
}
