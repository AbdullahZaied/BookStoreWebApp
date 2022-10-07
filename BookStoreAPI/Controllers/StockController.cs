using AutoMapper;
using BookStoreAPI.Models;
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
            return Ok(_mapper.Map<List<StockModelApi>>(stocklist));
        }
    }
}
