using AutoMapper;
using BookStoreAPI.Models;
using Business.Logic.Layer.Models;
using Business.Logic.Layer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public StockController(IStockService stockService, IMapper mapper)
        {
            _stockService = stockService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetStock()
        {
            var stocklist = await _stockService.GetStockAsync();
            if(stocklist == null)
            {
                return BadRequest("Empty Stock");
            }
            return Ok(_mapper.Map<List<StockModelApi>>(stocklist));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockById([FromRoute] int id)
        {
            var stockInfo = await _stockService.GetStockByIdAsync(id);
            if (stockInfo == null)
            {
                return BadRequest("Invalid Book Id");
            }
            else
            {
                return Ok(_mapper.Map<StockModelApi>(stockInfo));
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> SetStockById([FromBody] StockModelApi stock)
        {
            var bookId = await _stockService.SetStockByIdAsync(_mapper.Map<StockModelBusiness>(stock));
            if (bookId == null)
            {
                return BadRequest("Invalid Book Id");
            }
            else
            {
                return CreatedAtAction(nameof(GetStockById), new { id = bookId, controller = "Stock" }, bookId); ;
            }
        }
    }
}
