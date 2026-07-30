using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using INVERNADEROCONTULMO.Api.Models;
using INVERNADEROCONTULMO.Api.Services;

namespace INVERNADEROCONTULMO.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProveedoresController : ControllerBase
{
    private readonly IProveedorService _service;
    public ProveedoresController(IProveedorService service) => _service = service;
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));
    [HttpPost] public async Task<IActionResult> Create([FromBody] Proveedor p) => Ok(await _service.CreateAsync(p));
    [HttpPut("{id}")] public async Task<IActionResult> Update(int id, [FromBody] Proveedor p) => Ok(await _service.UpdateAsync(id, p));
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));
}
