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
        #region Create

        [Fact]
        [Trait("Category", "Create")]
        public void GivenCreate_WhenBookIsInvalid_ThenResultShouldBeFalse()
        {
            // Arrange
            var book = BookFake.CreateOneInvalid();
            var logger = new Mock<ILogger<BookService>>().Object;
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(databaseName: "Books").Options;
            using var context = new BookContext(options);

            var service = new BookService(logger, context);

            // Act
            var result = service.Create(book);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", "Create")]
        public void GivenCreate_WhenBookIsValidButAlreadyExistsInRepository_ThenResultShouldBeFalse()
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
            var result = service.Create(book);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", "Create")]
        public void GivenCreate_WhenBookIsValidAndDoesNotExistInRepository_ThenResultShouldBeTrue()
        {
            // Arrange
            var book = BookFake.CreateOneValid();
            var logger = new Mock<ILogger<BookService>>().Object;
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(databaseName: "Books").Options;
            using var context = new BookContext(options);

            var service = new BookService(logger, context);

            // Act
            var result = service.Create(book);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

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

        #region Update

        [Fact]
        [Trait("Category", "Update")]
        public void GivenUpdate_WhenBookIsInvalid_ThenResultShouldBeFalse()
        {
            // Arrange
            var book = BookFake.CreateOneInvalid();
            var logger = new Mock<ILogger<BookService>>().Object;
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(databaseName: "Books").Options;
            using var context = new BookContext(options);

            var service = new BookService(logger, context);

            // Act
            var result = service.Update(book);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", "Update")]
        public void GivenUpdate_WhenBookIsValidAndAlreadyExistsInRepository_ThenResultShouldBeTrue()
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
            book.Description = "Lorem ipsum dolor sit amet.";
            var result = service.Update(book);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "Update")]
        public void GivenCreate_WhenBookIsValidButDoesNotExistInRepository_ThenResultShouldBeFalse()
        {
            // Arrange
            var book = BookFake.CreateOneValid();
            var logger = new Mock<ILogger<BookService>>().Object;
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(databaseName: "Books").Options;
            using var context = new BookContext(options);

            var service = new BookService(logger, context);

            // Act
            var result = service.Update(book);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Delete

        [Fact]
        [Trait("Category", "Delete")]
        public void GivenDelete_WhenIsbnIsEmpty_ThenShouldReturnFalse()
        {
            // Arrange
            var isbn = string.Empty;
            var logger = new Mock<ILogger<BookService>>().Object;
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(databaseName: "Books").Options;
            using var context = new BookContext(options);

            var service = new BookService(logger, context);

            // Act
            var result = service.Delete(isbn);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", "Delete")]
        public void GivenDelete_WhenIsbnIdentifiesExistsingBook_ThenResultShouldBeTrue()
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
            var result = service.Delete(book.Isbn);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "Delete")]
        public void GivenDelete_WhenIsbnDoesNotIdentifyExistsingBook_ThenResultShouldBeFalse()
        {
            // Arrange
            var book = BookFake.CreateOneValid();
            var logger = new Mock<ILogger<BookService>>().Object;
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(databaseName: "Books").Options;
            using var context = new BookContext(options);

            var service = new BookService(logger, context);

            // Act
            var result = service.Delete(book.Isbn);

            // Assert
            result.Should().BeFalse();
        }

        #endregion
    }
}
