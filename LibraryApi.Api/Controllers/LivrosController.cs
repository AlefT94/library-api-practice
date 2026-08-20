using LibraryApi.Application.DTOs;
using LibraryApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LivrosController(ILivroService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LivroResponseDto>>> GetAll(CancellationToken cancellationToken)
        => Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LivroResponseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var livro = await service.GetByIdAsync(id, cancellationToken);
        return livro is null ? NotFound() : Ok(livro);
    }

    [HttpPost]
    public async Task<ActionResult<LivroResponseDto>> Create([FromBody] CreateLivroDto dto, CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLivroDto dto, CancellationToken cancellationToken)
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
