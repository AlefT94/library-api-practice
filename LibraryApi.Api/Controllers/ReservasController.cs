using LibraryApi.Application.DTOs;
using LibraryApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservasController(IReservaService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservaResponseDto>>> GetAll(CancellationToken cancellationToken)
        => Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReservaResponseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var reserva = await service.GetByIdAsync(id, cancellationToken);
        return reserva is null ? NotFound() : Ok(reserva);
    }

    [HttpPost]
    public async Task<ActionResult<ReservaResponseDto>> Create([FromBody] CreateReservaDto dto, CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReservaDto dto, CancellationToken cancellationToken)
    {
        var updated = await service.UpdateAsync(id, dto, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
