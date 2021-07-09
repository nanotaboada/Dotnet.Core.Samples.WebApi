using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Core.Samples.WebApi.Models
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options)
            : base(options)
        { }

        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// It also performs validation of Data Annotations.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the underlying database.
        /// This can include state entries for entities and/or relationships.
        /// Relationship state entries are created for many-to-many
        /// relationships and relationships where there is no foreign key
        /// property included in the entity class (often referred to as
        /// independent associations).
        /// </returns>
        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries()
                .Where(_ => _.State == EntityState.Added
                    || _.State == EntityState.Modified)
                .Select(_ => _.Entity);

            foreach (var entity in entities)
            {
                var context = new ValidationContext(entity);
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateObject(entity, context, results, true))
                {
                    foreach (var result in results)
                    {
                        if (result != ValidationResult.Success)
                        {
                            throw new ValidationException(result.ErrorMessage);
                        }
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}