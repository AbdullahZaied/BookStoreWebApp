using Business.Logic.Layer.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Logic.Layer.Services
{
    public interface IBookService
    {
        Task<List<BookModel>> getAllBooks();
        Task<BookModel> getBookById(int id);
        Task<int> addNewBook(BookModel book);
        Task<bool> updateBook(int id, BookModel book);
        Task<bool> updateBookPatch(int id, JsonPatchDocument book);
        Task<bool> deleteBookById(int id);

    }
}
