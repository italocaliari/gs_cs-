using Microsoft.AspNetCore.Mvc;
using Refresca.Api.Application.DTOs;
using Refresca.Api.Application.Interfaces;

namespace Refresca.Api.Controllers;

[ApiController]
[Route("api/alertas")]
public sealed class AlertasController(IRefrescaService service) : ControllerBase
{
    [HttpGet("hoje")]
    public async Task<ActionResult<AlertaHojeDto>> GetHoje(CancellationToken ct) =>
        Ok(await service.ObterAlertaHojeAsync(ct));
}
