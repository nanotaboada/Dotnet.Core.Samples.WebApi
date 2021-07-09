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
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        [Trait("Category", "HTTP GET")]
        public void GivenHttpGetVerb_WhenRequestParameterIdentifiesExistingBook_ThenShouldReturnStatusOkAndTheBook()
        {
            // Arrange
            var book = BookFake.CreateOneValid();
            var logger = new Mock<ILogger<BookController>>();

            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveByIsbn(book.Isbn)).Returns(book);

            var controller = new BookController(logger.Object, service.Object);

            // Act
            var result = controller.Get(book.Isbn) as OkObjectResult;

            // Assert
            service.Verify(s => s.RetrieveByIsbn(It.IsAny<string>()), Times.Exactly(1));
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(book);
        }
        #endregion

        #region HTTP POST

        [Fact]
        [Trait("Category", "HTTP POST")]
        public void GivenHttpPostVerb_WhenRequestBodyContainsExistingBook_ThenShouldReturnStatusConflict()
        {
            // Arrange
            var book = BookFake.CreateOneValid();
            var logger = new Mock<ILogger<BookController>>();

            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveByIsbn(book.Isbn)).Returns(book);
            service.Setup(s => s.Create(book));

            var controller = new BookController(logger.Object, service.Object);

            // Act
            var result = controller.Post(book) as ConflictResult;

            // Assert
            service.Verify(s => s.Create(It.IsAny<Book>()), Times.Never);
            result.StatusCode.Should().Be(409);
        }

        [Fact]
        [Trait("Category", "HTTP POST")]
        public void GivenHttpPostVerb_WhenRequestBodyContainsNewBook_ThenShouldReturnStatusCreatedAndLocationHeader()
        {
            // Arrange
            var book = BookFake.CreateOneValid();
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
            result.StatusCode.Should().Be(201);
            result.Location.Should().BeEquivalentTo(location);
            result.Value.Should().BeEquivalentTo(book);
        }

        [Fact]
        [Trait("Category", "HTTP POST")]
        public void GivenHttpPostVerb_WhenRequestBodyContainsInvalidBook_ThenShouldReturnStatusBadRequest()
        {
            // Arrange
            var book = BookFake.CreateOneInvalid();
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
            result.StatusCode.Should().Be(400);
        }

        #endregion

        #region HTTP PUT

        [Fact]
        [Trait("Category", "HTTP PUT")]
        public void GivenHttpPutVerb_WhenRequestBodyContainsExistingBook_ThenShouldReturnStatusNoContent()
        {
            // Arrange
            var book = BookFake.CreateOneValid();
            var logger = new Mock<ILogger<BookController>>();

            var service = new Mock<IBookService>();
            service.Setup(s => s.Update(book)).Returns(true);

            var controller = new BookController(logger.Object, service.Object);

            // Act
            var result = controller.Put(book) as NoContentResult;

            // Assert
            service.Verify(s => s.Update(It.IsAny<Book>()), Times.Exactly(1));
            result.StatusCode.Should().Be(204);
        }

        [Fact]
        [Trait("Category", "HTTP PUT")]
        public void GivenHttpPutVerb_WhenRequestBodyContainsNewBook_ThenShouldReturnNotFound()
        {
            // Arrange
            var book = BookFake.CreateOneValid();
            var logger = new Mock<ILogger<BookController>>();

            var service = new Mock<IBookService>();
            service.Setup(s => s.Update(book)).Returns(false);

            var controller = new BookController(logger.Object, service.Object);

            // Act
            var result = controller.Put(book) as NotFoundResult;

            // Assert
            service.Verify(s => s.Update(It.IsAny<Book>()), Times.Exactly(1));
            result.StatusCode.Should().Be(404);
        }

        #endregion

        #region HTTP DELETE

        [Fact]
        [Trait("Category", "HTTP DELETE")]
        public void GivenHttpDeleteVerb_WhenRequestParameterIdentifiesExistingBook_ThenShouldReturnStatusNoContent()
        {
            // Arrange
            var book = BookFake.CreateOneValid();
            var logger = new Mock<ILogger<BookController>>();

            var service = new Mock<IBookService>();
            service.Setup(s => s.Delete(book.Isbn)).Returns(true);

            var controller = new BookController(logger.Object, service.Object);

            // Act
            var result = controller.Delete(book.Isbn) as NoContentResult;

            // Assert
            service.Verify(s => s.Delete(It.IsAny<string>()), Times.Exactly(1));
            result.StatusCode.Should().Be(204);
        }

        [Fact]
        [Trait("Category", "HTTP DELETE")]
        public void GivenHttpDeleteVerb_WhenRequestParameterDoesNotIdentifyExistingBook_ThenShouldReturnNotFound()
        {
            // Arrange
            var book = BookFake.CreateOneValid();
            var logger = new Mock<ILogger<BookController>>();

            var service = new Mock<IBookService>();
            service.Setup(s => s.Delete(book.Isbn)).Returns(false);

            var controller = new BookController(logger.Object, service.Object);

            // Act
            var result = controller.Delete(book.Isbn) as NotFoundResult;

            // Assert
            service.Verify(s => s.Delete(It.IsAny<string>()), Times.Exactly(1));
            result.StatusCode.Should().Be(404);
        }

        #endregion
    }
}
