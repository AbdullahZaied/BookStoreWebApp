using AutoMapper;
using Business.Logic.Layer.Models;
using Data.Access.Layer.Data;
using Data.Access.Layer.Repository;
using Microsoft.AspNetCore.JsonPatch;

namespace Business.Logic.Layer.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _booKRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _booKRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<List<BookModel>> getAllBooks()
        {
            var bookList = await _booKRepository.getAllBooksAsync();
            return _mapper.Map<List<BookModel>>(bookList);
        }

        public async Task<BookModel> getBookById(int id)
        {
            var book = await _booKRepository.getBookByIdAsync(id);

            return _mapper.Map<BookModel>(book);
        }

        public async Task<int> addNewBook(BookModel book)
        {
            var id = await _booKRepository.addBookByIdAsync(_mapper.Map<Book>(book));
            return id;
        }

        public async Task<bool> updateBook(int id, BookModel book)
        {
            try
            {
                await _booKRepository.updateBookAsync(id, _mapper.Map<Book>(book));
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> updateBookPatch(int id, JsonPatchDocument book)
        {
            try
            {
                await _booKRepository.updateBookPatchAsync(id, book);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> deleteBookById(int id)
        {
            try
            {
                await _booKRepository.deleteBookByIdAsync(id);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}