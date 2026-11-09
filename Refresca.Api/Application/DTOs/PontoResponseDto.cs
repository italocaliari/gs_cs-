namespace Refresca.Api.Application.DTOs;

public sealed record UltimaLeituraDto(decimal IndiceCalor, string Nivel, DateTimeOffset MedidoEm);

public sealed record PontoResponseDto(
    int Id, string Nome, decimal Latitude, decimal Longitude,
    bool TemAgua, bool TemSombra, bool TemBanheiro, UltimaLeituraDto? UltimaLeitura);
