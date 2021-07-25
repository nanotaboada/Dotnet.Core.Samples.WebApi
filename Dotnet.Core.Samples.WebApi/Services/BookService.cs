using System;
using Dotnet.Core.Samples.WebApi.Models;
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
            var created = false;

            try
            {
                _bookContext.Add(book);
                created = _bookContext.SaveChanges() == 1;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }

            return created;
        }

        // Retrieve
        public Book RetrieveByIsbn(string isbn)
        {
            return _bookContext.Books.Find(isbn);
        }

        // Update
        public bool Update(Book book)
        {
            var updated = false;

            if (_bookContext.Books.Find(book.Isbn) is not null)
            {
                try
                {
                    _bookContext.Update(book);
                    updated = _bookContext.SaveChanges() == 1;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception.Message);
                }
            }

            return updated;
        }

        // Delete
        public bool Delete(string isbn)
        {
            var deleted = false;
            var existing = _bookContext.Books.Find(isbn);

            if (existing is not null)
            {
                try
                {
                    _bookContext.Books.Remove(existing);
                    deleted = _bookContext.SaveChanges() == 1;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception.Message);
                }
            }

            return deleted;
        }
    }
}
