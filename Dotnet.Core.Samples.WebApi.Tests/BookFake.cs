using System;
using Bogus;
using Dotnet.Core.Samples.WebApi.Models;

namespace Dotnet.Core.Samples.WebApi.Tests
{
    public static class BookFake
    {
        private static Random random = new Random();

        public static Book CreateOneValid()
        {
            // Required fields are: Isbn, Title, Author, Published, Description and Website.
            return new Faker<Book>()
                .RuleFor(book => book.Isbn, fake => CreateFakeIsbn())
                .RuleFor(book => book.Title, fake => fake.Lorem.Sentence(3))
                .RuleFor(book => book.Author, fake => fake.Name.FullName())
                .RuleFor(book => book.Published, fake => fake.Date.Past(6, DateTime.Now))
                .RuleFor(book => book.Description, fake => fake.Lorem.Paragraph())
                .RuleFor(book => book.Website, fake => fake.Internet.Url())
                .Generate();
        }

        public static Book CreateOneInvalid()
        {
            return new Faker<Book>()
                .RuleFor(book => book.Isbn, fake => CreateFakeIsbn())
                .RuleFor(book => book.Title, fake => fake.Lorem.Words(3).ToString())
                .RuleFor(book => book.Author, fake => fake.Name.FullName())
                .Generate();
        }

        private static string CreateFakeIsbn()
        {
            var ean = "978";
            var group = random.Next(0, 2).ToString("0");
            var publisher = random.Next(200, 699).ToString("000");
            var title = random.Next(0, 99999).ToString("00000");
            var check = random.Next(0, 10).ToString("0");

            return string.Format("{0}-{1}-{2}-{3}-{4}", ean, group, publisher, title, check);
        }
    }
}