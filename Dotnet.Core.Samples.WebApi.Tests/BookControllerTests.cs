using Dotnet.Core.Samples.WebApi.Controllers;
using Dotnet.Core.Samples.WebApi.Models;
using Dotnet.Core.Samples.WebApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dotnet.Core.Samples.WebApi.Tests
{
    public class BookControllerTests
    {
        #region HTTP GET
        [Fact]
        [Trait("Category", "HTTP GET")]
        public void GivenHttpGetVerb_WhenRequestParameterDoesNotIdentifyAnExistingBook_ThenShouldReturnStatusNotFound()
        {
            // Arrange
            string isbn = null;
            var logger = new Mock<ILogger<BookController>>();

            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveByIsbn(isbn)).Returns(null as Book);

            var controller = new BookController(logger.Object, service.Object);

            // Act
            var result = controller.Get(isbn) as NotFoundResult;

            // Assert
            service.Verify(s => s.RetrieveByIsbn(isbn), Times.Exactly(1));
            result.StatusCode.Should().Equals(404);
        }

        [Fact]
        [Trait("Category", "HTTP GET")]
        public void GivenHttpGetVerb_WhenRequestParameterIdentifiesAnExistingBook_ThenShouldReturnStatusOkAndTheBook()
        {
            // Arrange
            var book = BookFake.CreateOne();
            var logger = new Mock<ILogger<BookController>>();

            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveByIsbn(book.Isbn)).Returns(book);

            var controller = new BookController(logger.Object, service.Object);

            // Act
            var result = controller.Get(book.Isbn) as OkObjectResult;

            // Assert
            service.Verify(s => s.RetrieveByIsbn(It.IsAny<string>()), Times.Exactly(1));
            result.StatusCode.Should().Equals(200);
            result.Value.Should().BeEquivalentTo(book);
        }
        #endregion

        #region HTTP POST

        [Fact]
        [Trait("Category", "HTTP POST")]
        public void GivenHttpPostVerb_WhenRequestBodyContainsValidButExistingBook_ThenShouldReturnStatusConflict()
        {
            // Arrange
            var book = BookFake.CreateOne();
            var logger = new Mock<ILogger<BookController>>();

            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveByIsbn(book.Isbn)).Returns(book);
            service.Setup(s => s.Create(book));

            var controller = new BookController(logger.Object, service.Object);

            // Act
            var result = controller.Post(book) as ConflictResult;

            // Assert
            service.Verify(s => s.Create(It.IsAny<Book>()), Times.Never);
            result.StatusCode.Should().Equals(409);
        }

        [Fact]
        [Trait("Category", "HTTP POST")]
        public void GivenHttpPostVerb_WhenRequestBodyContainsValidNewBook_ThenShouldReturnStatusCreatedAndLocationHeader()
        {
            // Arrange
            var book = BookFake.CreateOne();
            var location = string.Format("/book/{0}", book.Isbn);
            var logger = new Mock<ILogger<BookController>>();

            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveByIsbn(book.Isbn)).Returns(null as Book);
            service.Setup(s => s.Create(book)).Returns(true);

            var controller = new BookController(logger.Object, service.Object);

            // Act
            var result = controller.Post(book) as CreatedResult;

            // Assert
            service.Verify(s => s.RetrieveByIsbn(It.IsAny<string>()), Times.Exactly(1));
            service.Verify(s => s.Create(It.IsAny<Book>()), Times.Exactly(1));
            result.StatusCode.Should().Equals(201);
            result.Location.Should().BeEquivalentTo(location);
            result.Value.Should().BeEquivalentTo(book);
        }

        [Fact]
        [Trait("Category", "HTTP POST")]
        public void GivenHttpPostVerb_WhenRequestBodyContainsInvalidBook_ThenShouldReturnStatusBadRequest()
        {
            // Arrange
            // The Book is invalid, as Title, Author, Published, Description and Website are required.
            var book = new Book { Isbn = "978-1484200773" };
            var location = string.Format("/book/{0}", book.Isbn);
            var logger = new Mock<ILogger<BookController>>();

            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveByIsbn(book.Isbn)).Returns(null as Book);
            service.Setup(s => s.Create(book)).Returns(false);

            var controller = new BookController(logger.Object, service.Object);

            // Act
            var result = controller.Post(book) as BadRequestResult;

            // Assert
            service.Verify(s => s.RetrieveByIsbn(It.IsAny<string>()), Times.Exactly(1));
            service.Verify(s => s.Create(It.IsAny<Book>()), Times.Exactly(1));
            result.StatusCode.Should().Equals(400);
        }

        #endregion

        // TODO: Add coverage for PUT and DELETE
    }
}
