using LibraryCatalog.Application.Queries.GetBooksList;
using Microsoft.AspNetCore.Mvc;

namespace LibraryCatalog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LibrosController : ControllerBase
{
    private readonly GetBooksListHandler _getBooksListHandler;

    public LibrosController(GetBooksListHandler getBooksListHandler)
    {
        _getBooksListHandler = getBooksListHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetList(CancellationToken cancellationToken)
    {
        var result = await _getBooksListHandler.HandleAsync(new GetBooksListQuery(), cancellationToken);

        return Ok(result);
    }
}
