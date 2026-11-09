using System.ComponentModel.DataAnnotations;

namespace Refresca.Api.Application.DTOs;

public sealed class RegistrarPausaDto
{
    [Required, StringLength(80, MinimumLength = 1)]
    public string TrabalhadorId { get; init; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int? PontoRefrescaId { get; init; }

    [Required]
    public DateTimeOffset RegistradaEm { get; init; }

    [Range(1, 180)]
    public int DuracaoMinutos { get; init; } = 10;
}
