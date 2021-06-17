using System;

namespace dotnet.core.samples.webapi.Models
{
    public static class BookFake
    {
        public static Book CreateOne()
        {
            return new Book
            {
                Isbn = "978-1484200773",
                Title = "Pro Git",
                SubTitle = "Everything you neeed to know about Git",
                Author = "Scott Chacon and Ben Straub",
                Published = new DateTime(2014, 11, 18, 0, 0, 0, DateTimeKind.Utc),
                Publisher = "Apress; 2nd edition",
                Pages = 458,
                Description = "Pro Git (Second Edition) is your fully-updated guide to Git and its usage in the modern world. " +
                    "Git has come a long way since it was first developed by Linus Torvalds for Linux kernel development. " +
                    "It has taken the open source world by storm since its inception in 2005, and this book teaches you how to use it like a pro.",
                Website = "https://git-scm.com/book/en/v2"
            };
        }
    }
}