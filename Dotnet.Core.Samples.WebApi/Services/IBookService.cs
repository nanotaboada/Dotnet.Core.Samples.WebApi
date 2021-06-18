using Dotnet.Core.Samples.WebApi.Models;

namespace Dotnet.Core.Samples.WebApi.Services
{
    public interface IBookService
    {
        // Create
        bool Create(Book book);

        // Retrieve
        Book RetrieveByIsbn(string isbn);

        // Update
        bool Update(Book book);

        // Delete
        bool Delete(string isbn);
    }
}
