using System;
using System.ComponentModel.DataAnnotations;

namespace Dotnet.Core.Samples.WebApi.Models
{
    public class Book
    {
        [Key][MinLength(13)][MaxLength(17)]
        public string Isbn { get; set; }

        [Required]
        public string Title { get; set; }

        public string SubTitle { get; set; }

        [Required]
        public string Author { get; set; }

        public string Publisher { get; set; }

        [Required]
        public DateTime Published { get; set; }

        public int Pages { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Website { get; set; }
    }
}
