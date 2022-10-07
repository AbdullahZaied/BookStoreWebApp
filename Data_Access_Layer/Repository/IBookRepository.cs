using Data.Access.Layer.Data;
using Microsoft.AspNetCore.JsonPatch;

namespace Data.Access.Layer.Repository
{
    public interface IBookRepository
    {
        Task<int> addBookByIdAsync(Book book);
        Task<List<Book>> getAllBooksAsync();
        Task<Book> getBookByIdAsync(int id);
        Task updateBookAsync(int bookId, Book book);
        Task updateBookPatchAsync(int bookId, JsonPatchDocument book);
        Task deleteBookByIdAsync(int id);
    }
}
