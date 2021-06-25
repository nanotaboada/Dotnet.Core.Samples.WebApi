using Dotnet.Core.Samples.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dotnet.Core.Samples.WebApi.Services
{
    public class BookService : IBookService
    {
        private readonly ILogger<BookService> _logger;
        private readonly BookContext _bookContext;

        public BookService(ILogger<BookService> logger, BookContext bookContext)
        {
            _logger = logger;
            _bookContext = bookContext;
        }

        // Create
        public bool Create(Book book)
        {
            int result;

            _bookContext.Add(book);

            // In EF Core we dropped doing validation since it is usually already
            // done client-side, server-side, and then in the database too.
            // https://github.com/dotnet/efcore/issues/5224
            result = _bookContext.SaveChanges();

            return result == 1;
        }

        // Retrieve
        public Book RetrieveByIsbn(string isbn)
        {
            return _bookContext.Books.Find(isbn);
        }

        // Update
        public bool Update(Book book)
        {
            int result = 0;

            if (_bookContext.Books.Find(book.Isbn) != null)
            {
                _bookContext.Update(book);
                result = _bookContext.SaveChanges();
            }

            return result == 1;
        }

        // Delete
        public bool Delete(string isbn)
        {
            int result = 0;
            var existing = _bookContext.Books.Find(isbn);

            if (existing != null)
            {
                _bookContext.Books.Remove(existing);
                result = _bookContext.SaveChanges();
            }

            return result == 1;
        }
    }
}
