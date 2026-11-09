namespace Refresca.Api.Application.DTOs;

public sealed record AlertaHojeDto(
    string Nivel, decimal? IndiceCalor, string Mensagem,
    DateTimeOffset? MedidoEm, string Fonte);
