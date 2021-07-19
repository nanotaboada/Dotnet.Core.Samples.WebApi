using Dotnet.Core.Samples.WebApi.Models;
using Dotnet.Core.Samples.WebApi.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dotnet.Core.Samples.WebApi.Tests
{
    /// <summary>
    /// "We use test doubles for internal testing of EF Core. However, we never
    /// try to mock DbContext or IQueryable. Doing so is difficult, cumbersome,
    /// and fragile. Don't do it.
    ///
    /// Instead we use the EF in-memory database when unit testing something
    /// that uses DbContext. In this case using the EF in-memory database is
    /// appropriate because the test is not dependent on database behavior."
    ///
    /// https://docs.microsoft.com/en-us/ef/core/testing/
    /// </summary>
    public class BookServiceTests
    {
        #region Retrieve

        [Fact]
        [Trait("Category", "Retrieve")]
        public void GivenRetrieveByIsbn_WhenIsbnIsFoundInContext_ThenShouldReturnTheBook()
        {
            // Arrange
            var book = BookFake.CreateOneValid();
            var logger = new Mock<ILogger<BookService>>().Object;
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(databaseName: "Books").Options;
            using var context = new BookContext(options);
            context.Books.Add(book);
            context.SaveChanges();

            var service = new BookService(logger, context);

            // Act
            var result = service.RetrieveByIsbn(book.Isbn);

            // Assert
            result.Should().BeEquivalentTo(book);
        }

        [Fact]
        [Trait("Category", "Retrieve")]
        public void GivenRetrieveByIsbn_WhenIsbnNotFoundInContext_ThenShouldReturnNull()
        {
            // Arrange
            var book = BookFake.CreateOneValid();
            var logger = new Mock<ILogger<BookService>>().Object;
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(databaseName: "Books").Options;
            using var context = new BookContext(options);

            var service = new BookService(logger, context);

            // Act
            var result = service.RetrieveByIsbn(book.Isbn);

            // Assert
            result.Should().BeNull();
        }

        #endregion
    }
}
