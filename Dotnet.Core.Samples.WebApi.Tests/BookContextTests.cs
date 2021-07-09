using System;
using System.ComponentModel.DataAnnotations;
using Dotnet.Core.Samples.WebApi.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dotnet.Core.Samples.WebApi.Tests
{
    public class BookContextTests
    {
        [Fact]
        public void GivenSaveChanges_WhenBookIsInvalid_ThenShouldThrowValidationException()
        {
            // Arrange
            var book = BookFake.CreateOneInvalid();
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(databaseName: "Books")
                .Options;

            var context = new BookContext(options);

            // Act
            context.Add(book);
            Action result = () => context.SaveChanges();

            // Assert
            result.Should().Throw<ValidationException>();
        }

        [Fact]
        public void GivenSaveChanges_WhenBookIsValid_ThenShouldReturnOne()
        {
            // Arrange
            var book = BookFake.CreateOneValid();
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(databaseName: "Books")
                .Options;

            var context = new BookContext(options);

            // Act
            context.Add(book);
            var result = context.SaveChanges();

            // Assert
            result.Should().Be(1);
        }
    }
}
