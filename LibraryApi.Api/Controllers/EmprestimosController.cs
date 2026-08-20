using LibraryApi.Application.DTOs;
using LibraryApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmprestimosController(IEmprestimoService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmprestimoResponseDto>>> GetAll(CancellationToken cancellationToken)
        => Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmprestimoResponseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var emprestimo = await service.GetByIdAsync(id, cancellationToken);
        return emprestimo is null ? NotFound() : Ok(emprestimo);
    }

    [HttpPost]
    public async Task<ActionResult<EmprestimoResponseDto>> Create([FromBody] CreateEmprestimoDto dto, CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmprestimoDto dto, CancellationToken cancellationToken)
    {
        var updated = await service.UpdateAsync(id, dto, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpPost("{id:int}/devolucao")]
    public async Task<ActionResult<EmprestimoResponseDto>> RegistrarDevolucao(int id, [FromQuery] DateOnly? dataDevolucaoReal, CancellationToken cancellationToken)
    {
        var result = await service.RegistrarDevolucaoAsync(id, dataDevolucaoReal, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
