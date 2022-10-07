using Business.Logic.Layer.Models;
using Business.Logic.Layer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("")]
        public async Task<IActionResult> getAllBooks()
        {
            var books = await _bookService.getAllBooks();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getBookById([FromRoute] int id)
        {
            var book = await _bookService.getBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("")]
        public async Task<IActionResult> addNewBook([FromBody] BookModel book)
        {
            var id = await _bookService.addNewBook(book);
            return CreatedAtAction(nameof(getBookById), new { id = id, controller = "Book" }, id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> updateBook([FromRoute] int id, [FromBody] BookModel book)
        {
            var isUpdated = await _bookService.updateBook(id, book);
            if(isUpdated)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
            
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> updateBookPatch([FromRoute] int id, [FromBody] JsonPatchDocument book)
        {
            var isUpdated = await _bookService.updateBookPatch(id, book);
            if (isUpdated)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteBookById([FromRoute] int id)
        {
            var isDeleted = await _bookService.deleteBookById(id);
            if(isDeleted)
            {
                return Ok();
            }
            else
            { 
                return BadRequest();
            }
        }
    }
}
