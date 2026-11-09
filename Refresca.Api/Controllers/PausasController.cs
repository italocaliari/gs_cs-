using Microsoft.AspNetCore.Mvc;
using Refresca.Api.Application.DTOs;
using Refresca.Api.Application.Interfaces;

namespace Refresca.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class PausasController(IRefrescaService service) : ControllerBase
{
    [HttpPost("pausas")]
    [ProducesResponseType(typeof(PausaResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<PausaResponseDto>> Post(
        [FromBody] RegistrarPausaDto dto, CancellationToken ct)
    {
        var result = await service.RegistrarPausaAsync(dto, ct);
        return Created($"/api/trabalhadores/{result.TrabalhadorId}/pausas/{result.Id}", result);
    }

    [HttpGet("trabalhadores/{id}/pausas")]
    public async Task<ActionResult<IReadOnlyList<PausaResponseDto>>> GetByWorker(
        string id, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(id)) return BadRequest("Identificador inválido.");
        return Ok(await service.ListarPausasAsync(id, ct));
    }
}
