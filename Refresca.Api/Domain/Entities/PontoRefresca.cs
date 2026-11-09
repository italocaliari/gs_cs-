namespace Refresca.Api.Domain.Entities;

public sealed class PontoRefresca
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    public bool TemAgua { get; private set; }
    public bool TemSombra { get; private set; }
    public bool TemBanheiro { get; private set; }
    public string TotemId { get; private set; } = string.Empty;
    public ICollection<Leitura> Leituras { get; private set; } = new List<Leitura>();

    private PontoRefresca() { }

    public PontoRefresca(string nome, decimal latitude, decimal longitude,
        bool temAgua, bool temSombra, bool temBanheiro, string totemId)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome obrigatório.");
        if (latitude is < -90 or > 90) throw new ArgumentOutOfRangeException(nameof(latitude));
        if (longitude is < -180 or > 180) throw new ArgumentOutOfRangeException(nameof(longitude));
        if (string.IsNullOrWhiteSpace(totemId)) throw new ArgumentException("Totem obrigatório.");

        Nome = nome.Trim();
        Latitude = latitude;
        Longitude = longitude;
        TemAgua = temAgua;
        TemSombra = temSombra;
        TemBanheiro = temBanheiro;
        TotemId = totemId.Trim();
    }
}
