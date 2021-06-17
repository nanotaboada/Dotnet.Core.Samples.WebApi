using dotnet.core.samples.webapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnet.core.samples.webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBookService _bookService;

        public BookController(ILogger<BookController> logger, IBookService bookService)
        {
            this._logger = logger;
            this._bookService = bookService;
        }

        [Route("api/book/{isbn}")]
        [HttpGet]
        public IActionResult Get(string isbn)
        {
            var book = this._bookService.RetrieveByIsbn(isbn);

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(book);
            }
        }
    }
}
