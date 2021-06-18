using System;
using dotnet.core.samples.webapi.Models;
using Microsoft.Extensions.Logging;

namespace dotnet.core.samples.webapi.Services
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
            _bookContext.Add(book);
            int result = _bookContext.SaveChanges();

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
