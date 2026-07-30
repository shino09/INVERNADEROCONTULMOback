using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using INVERNADEROCONTULMO.Api.DTOs;
using INVERNADEROCONTULMO.Api.Reports;
using INVERNADEROCONTULMO.Api.Services;

namespace INVERNADEROCONTULMO.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VentasController : ControllerBase
{
    // Servicio de ventas
    private readonly IVentaService _ventaService;
    // Servicio de reportes PDF
    private readonly IReportService _reportService;
    // Constructor que inyecta dependencias
    public VentasController(IVentaService ventaService, IReportService reportService) { _ventaService = ventaService; _reportService = reportService; }

    // Obtiene todas las ventas
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _ventaService.GetAllAsync());
    // Obtiene una venta por su identificador
    [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) => Ok(await _ventaService.GetByIdAsync(id));

    // Crea una nueva venta con descuento de stock
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VentaDTO dto)
    {
        var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _ventaService.CreateAsync(dto, uid));
    }

    // Genera y descarga la factura PDF de una venta
    [HttpGet("{id}/factura")]
    public async Task<IActionResult> GetFactura(int id)
    {
        var venta = await _ventaService.GetByIdAsync(id);
        if (venta == null) return NotFound();
        return File(_reportService.GenerateFacturaPdf(venta), "application/pdf", $"Factura_{venta.NumeroFactura}.pdf");
    }
}
