using LibraryCatalog.Application.Queries.GetBookById;
using LibraryCatalog.Application.Queries.GetBooksByCategory;
using LibraryCatalog.Application.Queries.GetBooksList;
using Microsoft.AspNetCore.Mvc;

namespace LibraryCatalog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LibrosController : ControllerBase
{
    private readonly GetBooksListHandler _getBooksListHandler;
    private readonly GetBookByIdHandler _getBookByIdHandler;
    private readonly GetBooksByCategoryHandler _getBooksByCategoryHandler;

    public LibrosController(
        GetBooksListHandler getBooksListHandler,
        GetBookByIdHandler getBookByIdHandler,
        GetBooksByCategoryHandler getBooksByCategoryHandler)
    {
        _getBooksListHandler = getBooksListHandler;
        _getBookByIdHandler = getBookByIdHandler;
        _getBooksByCategoryHandler = getBooksByCategoryHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetList(CancellationToken cancellationToken)
    {
        var result = await _getBooksListHandler.HandleAsync(new GetBooksListQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getBookByIdHandler.HandleAsync(new GetBookByIdQuery(id), cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("categoria/{categoriaId}")]
    public async Task<IActionResult> GetByCategoria(Guid categoriaId, CancellationToken cancellationToken)
    {
        var result = await _getBooksByCategoryHandler.HandleAsync(new GetBooksByCategoryQuery(categoriaId), cancellationToken);

        return Ok(result);
    }
}