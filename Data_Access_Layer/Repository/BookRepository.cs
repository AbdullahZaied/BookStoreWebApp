using Data.Access.Layer.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace Data.Access.Layer.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _dbContext;

        public BookRepository(BookStoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Book>> getAllBooksAsync()
        {
            var booklist = await _dbContext.Books.ToListAsync();

            return booklist;
        }
        public async Task<Book> getBookByIdAsync(int bookId)
        {
            var book = await _dbContext.Books.FindAsync(bookId);
            
            return book;
        }

        public async Task<int> addBookByIdAsync(Book book)
        {
            var newBook = new Book()
            {
                BookName = book.BookName,
                Author = book.Author,
                Category = book.Category
            };

            _dbContext.Books.Add(newBook);
            await _dbContext.SaveChangesAsync();

            return newBook.Id;
        }

        public async Task updateBookAsync(int bookId, Book book)
        {
            var theBook = new Book()
            {
                Id = bookId,
                BookName = book.BookName,
                Author = book.Author,
                Category = book.Category
            };

            _dbContext.Books.Update(theBook);
            await _dbContext.SaveChangesAsync();
        }

        public async Task updateBookPatchAsync(int bookId, JsonPatchDocument book)
        {
            var TheBook = await _dbContext.Books.FindAsync(bookId);
            if (TheBook != null)
            {
                book.ApplyTo(TheBook);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task deleteBookByIdAsync(int id)
        {
            var theBook = new Book() { Id = id };
            _dbContext.Books.Remove(theBook);

            await _dbContext.SaveChangesAsync();
        }
    }
}
