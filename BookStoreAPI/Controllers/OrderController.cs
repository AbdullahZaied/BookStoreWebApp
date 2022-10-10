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
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService,
            IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost("")]
        public async Task<IActionResult> OrderBookById([FromBody] OrderModelApi order)
        {
            var orderStatus = await _orderService.OrderBookById(_mapper.Map<OrderModelBusiness>(order));
            
            if(orderStatus == -1)
            {
                return BadRequest();
            }
            else
            {
                return Ok(orderStatus);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById([FromRoute] int id)
        {
            var order = _mapper.Map<OrderModelApi>(await _orderService.GetOrderByIdAsync(id));
            if(order == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(order);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderById([FromRoute] int id)
        {
            var deleted = await _orderService.DeleteOrderByIdAsync(id);

            if (deleted != null)
            {
                return Ok(deleted);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
