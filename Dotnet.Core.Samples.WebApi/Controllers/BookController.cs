using dotnet.core.samples.webapi.Models;
using dotnet.core.samples.webapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnet.core.samples.webapi.Controllers
{
    /// <summary>
    /// 
    /// <para>
    /// The <b>HTTP GET</b> method requests a representation of the specified
    /// resource. Requests using GET should only retrieve data.
    /// </para>
    /// 
    /// <para>
    /// The <b>HTTP POST</b> method is used to submit an entity to the specified
    /// resource.
    /// </para>
    /// 
    /// <para>
    /// The <b>HTTP PUT</b> method replaces all current representations of the
    /// target resource with the request payload.
    /// </para>
    /// 
    /// <para>
    /// The <b>HTTP DELETE</b> method deletes the specified resource.
    /// </para>
    ///
    /// See: <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods">
    /// HTTP request methods - HTTP | MDN
    /// </see>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBookService _bookService;

        public BookController(ILogger<BookController> logger, IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        #region HTTP POST
        /// <summary>
        /// 
        /// <para>
        /// The <b>HTTP POST</b> method sends data to the server. The type of
        /// the body of the request is indicated by the Content-Type header.
        /// </para>
        /// 
        /// <para>
        /// The difference between PUT and POST is that PUT is idempotent:
        /// calling it once or several times successively has the same effect
        /// (that is, no side effect), where successive identical POST may have
        /// additional effects, like passing an order several times.
        /// </para>
        /// 
        /// </summary>
        ///
        /// <param name="book">The Book to be created.</param>
        /// 
        /// <returns>
        ///
        /// <para>
        /// <b>409 (Conflict)</b> if the Book already exists.
        /// </para>
        /// 
        /// <para>
        /// <b>201 (Created)</b> including the URI of the new Book in the
        /// Location header.
        /// </para>
        ///
        /// <para>
        /// <b>400 (Bad Request)</b> if the request data is invalid.
        /// </para>
        /// 
        /// </returns>
        [Route("/book")]
        [HttpPost]
        public IActionResult Post(Book book)
        {
            if (_bookService.RetrieveByIsbn(book.Isbn) != null)
            {
                return Conflict();
            }
            else
            {
                if (_bookService.Create(book))
                {
                    var location = string.Format("/book/{0}", book.Isbn);
                    return Created(location, book);
                }
                else
                {
                    return BadRequest();
                }
            }
        }
        #endregion

        #region HTTP GET
        /// <summary>
        /// 
        /// The <b>HTTP GET</b> method requests a representation of the
        /// specified resource. Requests using GET should only retrieve data.
        /// 
        /// </summary>
        /// 
        /// <param name="isbn">The ISBN of the Book to retrieve.</param>
        ///
        /// <returns>
        ///
        /// <para>
        /// <b>200 (OK)</b> and the Book matching the provided ISBN.
        /// </para>
        ///
        /// <para>
        /// <b>404 (Not Found)</b> if the provided ISBN does not match a Book.
        /// </para>
        ///
        /// </returns>
        [Route("/book/{isbn}")]
        [HttpGet]
        public IActionResult Get(string isbn)
        {
            var book = _bookService.RetrieveByIsbn(isbn);

            if (book != null)
            {
                return Ok(book);
            }
            else
            {
                return NotFound();
            }
        }
        #endregion

        #region HTTP PUT
        /// <summary>
        /// 
        /// The <b>PUT</b> method replaces all current representations of the
        /// target resource with the request payload.
        /// 
        /// </summary>
        /// 
        /// <param name="book">The Book to be updated.</param>
        /// 
        /// <returns>
        ///
        /// <para>
        /// <b>204 (No Content)</b> if the Book as successfully been updated.
        /// </para>
        ///
        /// <para>
        /// <b>404 (Not Found)</b> if the provided Book does not exist.
        /// </para>
        /// 
        /// </returns>
        [Route("/book")]
        [HttpPut]
        public IActionResult Put(Book book)
        {
            if (_bookService.Update(book))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
        #endregion

        #region HTTP DELETE
        /// <summary>
        /// 
        /// The <b>HTTP DELETE</b> method deletes the specified resource.
        ///
        /// </summary>
        /// 
        /// <param name="book">The Book to be deleted.</param>
        /// 
        /// <returns>
        ///
        /// <para>
        /// <b>204 (No Content)</b> if the Book as successfully been deleted.
        /// </para>
        ///
        /// <para>
        /// <b>404 (Not Found)</b> if the provided Book does not exist.
        /// </para>
        /// 
        /// </returns>
        [Route("/book/{isbn}")]
        [HttpDelete]
        public IActionResult Delete(string isbn)
        {
            if (_bookService.Delete(isbn))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
        #endregion
    }
}
