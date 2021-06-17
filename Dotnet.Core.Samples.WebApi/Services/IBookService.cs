using dotnet.core.samples.webapi.Models;

namespace dotnet.core.samples.webapi.Services
{
    public interface IBookService
    {
        // Create
        int Create(Book book);

        // Retrieve
        Book RetrieveByIsbn(string isbn);

        // Update
        void Update(Book book);

        // Delete
        void Delete(string isbn);
    }
}
