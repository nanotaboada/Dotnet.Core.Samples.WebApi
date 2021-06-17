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
            this._logger = logger;
            this._bookContext = bookContext;
        }

        // Create
        public int Create(Book book)
        {
            throw new NotImplementedException();
        }

        // Retrieve
        public Book RetrieveByIsbn(string isbn)
        {
            return _bookContext.Books.Find(isbn);
        }

        // Update
        public void Update(Book book)
        {
            throw new NotImplementedException();
        }

        // Delete
        public void Delete(string isbn)
        {
            throw new NotImplementedException();
        }
    }
}
