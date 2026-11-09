using Refresca.Api.Application.DTOs;

namespace Refresca.Api.Application.Interfaces;

public interface IRefrescaService
{
    Task<LeituraResponseDto> RegistrarLeituraAsync(RegistrarLeituraDto dto, CancellationToken ct);
    Task<IReadOnlyList<PontoResponseDto>> ListarPontosAsync(CancellationToken ct);
    Task<PontoResponseDto?> ObterPontoAsync(int id, CancellationToken ct);
    Task<PausaResponseDto> RegistrarPausaAsync(RegistrarPausaDto dto, CancellationToken ct);
    Task<IReadOnlyList<PausaResponseDto>> ListarPausasAsync(string trabalhadorId, CancellationToken ct);
    Task<AlertaHojeDto> ObterAlertaHojeAsync(CancellationToken ct);
}
