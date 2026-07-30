using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using INVERNADEROCONTULMO.Api.Models;
using INVERNADEROCONTULMO.Api.Reports;
using INVERNADEROCONTULMO.Api.Services;

namespace INVERNADEROCONTULMO.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContabilidadController : ControllerBase
{
    // Servicio de contabilidad
    private readonly IContabilidadService _service;
    // Servicio de reportes PDF
    private readonly IReportService _reportService;
    // Constructor que inyecta dependencias
    public ContabilidadController(IContabilidadService service, IReportService reportService) { _service = service; _reportService = reportService; }

    // Obtiene todos los asientos contables
    [HttpGet("asientos")] public async Task<IActionResult> GetAsientos() => Ok(await _service.GetAllAsientosAsync());

    // Crea un nuevo asiento contable (solo Admin)
    [HttpPost("asientos")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateAsiento([FromBody] AsientoContable a) => Ok(await _service.CreateAsientoAsync(a));

    // Obtiene el libro diario, opcionalmente en PDF
    [HttpGet("libro-diario")]
    public async Task<IActionResult> GetLibroDiario([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    {
        var asientos = await _service.GetLibroDiarioAsync(desde, hasta);
        if (Request.Query["format"].FirstOrDefault() == "pdf")
            return File(_reportService.GenerateLibroDiarioPdf(asientos, desde, hasta), "application/pdf", "LibroDiario.pdf");
        return Ok(asientos);
    }

    // Obtiene el libro mayor, opcionalmente en PDF
    [HttpGet("libro-mayor")]
    public async Task<IActionResult> GetLibroMayor([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    {
        var cuentas = await _service.GetLibroMayorAsync(desde, hasta);
        if (Request.Query["format"].FirstOrDefault() == "pdf")
            return File(_reportService.GenerateLibroMayorPdf(cuentas, desde, hasta), "application/pdf", "LibroMayor.pdf");
        return Ok(cuentas);
    }
}
