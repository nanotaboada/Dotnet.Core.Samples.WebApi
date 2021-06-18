using dotnet.core.samples.webapi.Models;

namespace dotnet.core.samples.webapi.Services
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
