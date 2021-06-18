using dotnet.core.samples.webapi.Controllers;
using dotnet.core.samples.webapi.Models;
using dotnet.core.samples.webapi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace dotnet.core.samples.webapi.tests
{
    public class BookControllerTests
    {
        #region HTTP GET
        [Fact]
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


    }
}
