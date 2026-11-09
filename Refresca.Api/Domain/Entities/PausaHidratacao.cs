namespace Refresca.Api.Domain.Entities;

public sealed class PausaHidratacao
{
    public int Id { get; private set; }
    public string TrabalhadorId { get; private set; } = string.Empty;
    public int? PontoRefrescaId { get; private set; }
    public DateTimeOffset RegistradaEm { get; private set; }
    public int DuracaoMinutos { get; private set; }
    public PontoRefresca? PontoRefresca { get; private set; }

    private PausaHidratacao() { }

    public PausaHidratacao(string trabalhadorId, int? pontoRefrescaId,
        DateTimeOffset registradaEm, int duracaoMinutos)
    {
        if (string.IsNullOrWhiteSpace(trabalhadorId))
            throw new ArgumentException("O identificador do trabalhador é obrigatório.");
        if (duracaoMinutos is < 1 or > 180)
            throw new ArgumentOutOfRangeException(nameof(duracaoMinutos),
                "A duração deve estar entre 1 e 180 minutos.");

        TrabalhadorId = trabalhadorId.Trim();
        PontoRefrescaId = pontoRefrescaId;
        RegistradaEm = registradaEm;
        DuracaoMinutos = duracaoMinutos;
    }
}
