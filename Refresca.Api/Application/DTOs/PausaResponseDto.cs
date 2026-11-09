namespace Refresca.Api.Application.DTOs;

public sealed record PausaResponseDto(
    int Id, string TrabalhadorId, int? PontoRefrescaId,
    DateTimeOffset RegistradaEm, int DuracaoMinutos);
